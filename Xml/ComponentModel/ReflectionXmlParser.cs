using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Raccoom.Xml.ComponentModel
{
    /// <summary>
    /// Generic xml parser. Works like OR Mapper, reflects target object and set corresponding values from xml file.
    /// </summary>
    public class SyndicationObjectParser
    {
        #region fields
        /// <summary></summary>
        System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo>> _complexTypeRepository;
        #endregion

        #region internal interface
        /// <summary>Xml parsing against reflected target object</summary>
        /// <param name="target">Target object instance that will hold the gained data</param>
        /// <param name="xmlTextReader">XmlReader instance that holds the source xml</param>
        /// <exception cref="XmlException">The exception that is thrown on XML parse errors. </exception>
        /// <exception cref="System.IO.IOException">The exception that is thrown when an I/O error occurs.</exception>
        protected virtual void Parse(object target, System.Xml.XmlReader xmlTextReader)
        {
            Parse(target, xmlTextReader, null);
        }
        protected virtual void Parse(object target, System.Xml.XmlReader xmlTextReader, IParseableObject parseableObject)
        {
            try
            {
                IExtendableObject expandableTarget = target as IExtendableObject;
                if (target is IParseableObject)
                {
                    ((IParseableObject)target).BeginParse();
                    parseableObject = (IParseableObject)target;
                }
                //
                int depth = xmlTextReader.Depth;
                // cache meta info to gain performance
                System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo> complexProperties = GetComplexProperties(target);
                // initalize
                System.Reflection.PropertyInfo pi = null;
                string name;
                object childObject;
                XmlReader r = null;
                XmlNode node = null, childNode = null;
                // process target
                while (!xmlTextReader.EOF)
                {
                    if (xmlTextReader.ReadState == ReadState.Error) return;
                    xmlTextReader.Read();
                    xmlTextReader.MoveToContent();
                    //
                    if (xmlTextReader.NodeType == XmlNodeType.EndElement || xmlTextReader.NodeType == XmlNodeType.None) continue;
                    // skip elements with namespace ( extensions )
                    if (!string.IsNullOrEmpty(xmlTextReader.Prefix) & expandableTarget == null) continue;
                    //
                    name = xmlTextReader.LocalName;

                    // value of element strictly mapped to Value property _> rss guid
                    if (string.IsNullOrEmpty(name)) name = "Value";
                    // is it a complex type like item or source
                    if (complexProperties.ContainsKey(name) & depth < xmlTextReader.Depth)
                    {
                        // is it a collection based item and needs to be created and added?
                        if (complexProperties[name].PropertyType.GetInterface(typeof(System.Collections.IList).Name) != null)
                        {
                            // get create item method
                            System.Reflection.MethodInfo mi = target.GetType().GetMethod("Create" + name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            // invoke create method
                            childObject = mi.Invoke(target, null);
                            // add item
                            complexProperties[name].PropertyType.InvokeMember("Add", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, complexProperties[name].GetValue(target, null), new object[] { childObject });
                        }
                        else
                        {
                            // get existing instance from target
                            childObject = complexProperties[name].GetValue(target, null);
                        }
                        // recursive call to parse child object
                        Parse(childObject, xmlTextReader.ReadSubtree(), parseableObject);
                    }
                    else
                    {
                        // Check namespace
                        if (!string.IsNullOrEmpty(xmlTextReader.Prefix) & xmlTextReader.NodeType == XmlNodeType.Element)
                        {
                            r = xmlTextReader.ReadSubtree();
                            //
                            while (!r.EOF)
                            {
                                r.Read();
                                if (r.NodeType == XmlNodeType.EndElement || r.NodeType == XmlNodeType.None) continue;
                                node = expandableTarget.ExtendedContent.OwnerDocument.CreateNode(r.NodeType, r.Prefix, r.LocalName, r.NamespaceURI);
                                if (r.IsEmptyElement) continue;
                                r.Read();
                                childNode = expandableTarget.ExtendedContent.OwnerDocument.CreateNode(r.NodeType, r.Prefix, r.LocalName, r.NamespaceURI);
                                childNode.Value = System.Web.HttpUtility.HtmlEncode(r.Value);
                                node.AppendChild(childNode);
                            }
                            expandableTarget.ExtendedContent.AppendChild(node);
                            //
                            continue;
                        }
                        // get attributes if any exists
                        if (xmlTextReader.HasAttributes)
                        {
                            while (xmlTextReader.MoveToNextAttribute())
                            {
                                try
                                {
                                    // xmlns
                                    if (xmlTextReader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/") & xmlTextReader.LocalName != "xmlns" & expandableTarget != null)
                                    {
                                        expandableTarget.Xmlns.Add(xmlTextReader.LocalName, xmlTextReader.Value);
                                    }
                                    else
                                    {
                                        // try to get attribute property
                                        pi = GetPropertyByName(xmlTextReader.Name, target);
                                        if (pi == null) continue;
                                        // get attribute value
                                        SetPropertyValue(xmlTextReader, target, pi);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if (parseableObject != null) parseableObject.Errors.Add(target.GetType().Name + "." + pi.Name, e);
                                }
                            }
                        }
                        // empty elements have no value, even if they exists as property on the class move on
                        if (xmlTextReader.IsEmptyElement) continue;
                        // try to get element property
                        pi = GetPropertyByName(name, target);
                        if (pi == null) continue;
                        // read if not already done
                        if (xmlTextReader.NodeType == XmlNodeType.Element) xmlTextReader.Read();
                        // get element value
                        try
                        {
                            SetPropertyValue(xmlTextReader, target, pi);
                        }
                        catch (Exception e)
                        {
                            if (parseableObject != null)
                            {
                                // try to find a unique key name
                                string key = target.GetType().Name + "." + pi.Name;
                                int i = 1;
                                while (parseableObject.Errors.ContainsKey(key))
                                {
                                    key += i;
                                    i++;
                                }
                                // add item with unique key to the list
                                parseableObject.Errors.Add(key, e);
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
                throw;
            }
            catch (System.IO.IOException)
            {
                throw;
            }
            catch (System.Exception e)
            {
                if (parseableObject != null) parseableObject.Errors.Add(target.GetType().Name, e);
            }
            finally
            {
                //
                if (target is IParseableObject)
                {
                    ((IParseableObject)target).EndParse();
                }                
            }
        }
        /// <summary>Reflects all the complex properties contained in target</summary>
        protected virtual System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo> GetComplexProperties(object target)
        {
            System.Collections.Generic.Dictionary<string, System.Reflection.PropertyInfo> complexProperties = null;
            // reflect complex tags
            if (_complexTypeRepository == null) _complexTypeRepository = new System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<string, PropertyInfo>>();
            if (_complexTypeRepository.ContainsKey(target.GetType())) return _complexTypeRepository[target.GetType()];
            complexProperties = new System.Collections.Generic.Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (System.Reflection.PropertyInfo pInfo in target.GetType().GetProperties())
            {

                if (!pInfo.PropertyType.Assembly.Equals(target.GetType().Assembly)) continue;
                if (pInfo.PropertyType.IsEnum) continue;
                //
                object[] attributes = pInfo.GetCustomAttributes(typeof(System.Xml.Serialization.XmlElementAttribute), true);
                if (attributes.Length == 0) continue;
                //
                System.Xml.Serialization.XmlElementAttribute xmlAttribute = attributes[0] as System.Xml.Serialization.XmlElementAttribute;
                if (xmlAttribute == null) continue;
                //
                complexProperties.Add(xmlAttribute.ElementName + xmlAttribute.Namespace, pInfo);
            }
            //
            _complexTypeRepository.Add(target.GetType(), complexProperties);
            return complexProperties;
        }
        /// <summary>
        ///  Try to get property by name
        /// </summary>
        protected virtual System.Reflection.PropertyInfo GetPropertyByName(string name, object target)
        {
            return target.GetType().GetProperty(name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }
        /// <summary>
        /// Set property value 
        /// </summary>
        protected virtual void SetPropertyValue(System.Xml.XmlReader xmlTextReader, object target, System.Reflection.PropertyInfo pi)
        {
            switch (xmlTextReader.NodeType)
            {
                case XmlNodeType.Attribute:
                case XmlNodeType.CDATA:
                case XmlNodeType.Text:
                    string content = xmlTextReader.ReadContentAsString().Trim();
                    if (pi.PropertyType.Equals(typeof(DateTime)))
                    {
                        pi.SetValue(target, ParseDateTime(content), null);
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        // try to set enum value
                        if (System.Enum.IsDefined(pi.PropertyType, content))
                        {
                            pi.SetValue(target, System.ComponentModel.TypeDescriptor.GetConverter(pi.PropertyType).ConvertFromString(content), null);
                        }
                        else
                        {
                            // take the long way, we have to look up xml enum substitute name
                            System.Reflection.FieldInfo[] fields = pi.PropertyType.GetFields();
                            for (int i = 1; i < fields.Length; i++)
                            {
                                System.Reflection.FieldInfo field = fields[i];
                                object[] attributes = field.GetCustomAttributes(false);
                                if (attributes.Length == 0) continue;
                                //
                                System.Xml.Serialization.XmlEnumAttribute enumAttribute = attributes[0] as System.Xml.Serialization.XmlEnumAttribute;
                                if (enumAttribute == null) continue;
                                //
                                if (!StringComparer.OrdinalIgnoreCase.Equals(enumAttribute.Name, content)) continue;
                                // set substituted enum value
                                pi.SetValue(target, System.ComponentModel.TypeDescriptor.GetConverter(pi.PropertyType).ConvertFromString(field.Name), null);
                                break;
                            }
                        }
                    }
                    else if (pi.PropertyType.Equals(typeof(string)))
                    {
                        pi.SetValue(target, System.Web.HttpUtility.HtmlDecode(content), null);
                    }
                    else
                    {
                        pi.SetValue(target, System.ComponentModel.TypeDescriptor.GetConverter(pi.PropertyType).ConvertFromString(content), null);
                    }
                    break;
            }
        }
        #region rfs datetime

        /// <summary>
        /// <remarks>
        ///Copyright (c)  vendredi13@007.freesurf.fr
        ///All rights reserved.
        /// </remarks>
        /// </summary>
        /// <param name="adate">rfc date time</param>
        /// <returns>Rfc date as DateTime</returns>
        public static System.DateTime ParseDateTime(string adate)
        {
            System.DateTime dt = System.DateTime.Now;
            if (DateTime.TryParse(adate, out dt)) return dt;
            //
            string tmp;
            string[] resp;
            string dayName;
            string dpart;
            string hour, minute;
            string timeZone;

            //--- strip comments
            //--- XXX : FIXME : how to handle nested comments ?
            tmp = Regex.Replace(adate, "(\\([^(].*\\))", "");

            // strip extra white spaces
            tmp = Regex.Replace(tmp, "\\s+", " ");
            tmp = Regex.Replace(tmp, "^\\s+", "");
            tmp = Regex.Replace(tmp, "\\s+$", "");

            // extract week name part
            resp = tmp.Split(new char[] { ',' }, 2);
            if (resp.Length == 2)
            {
                // there's week name
                dayName = resp[0];
                tmp = resp[1];
            }
            else dayName = "";

            try
            {
                // extract date and time
                int pos = tmp.LastIndexOf(" ");
                if (pos < 1) throw new FormatException("probably not a date");
                dpart = tmp.Substring(0, pos - 1);
                timeZone = tmp.Substring(pos + 1);
                dt = Convert.ToDateTime(dpart);

                // check weekDay name
                // this must be done befor convert to GMT 
                if (!string.IsNullOrEmpty(dayName))
                {
                    if ((dt.DayOfWeek == DayOfWeek.Friday && dayName != "Fri") ||
                        (dt.DayOfWeek == DayOfWeek.Monday && dayName != "Mon") ||
                        (dt.DayOfWeek == DayOfWeek.Saturday && dayName != "Sat") ||
                        (dt.DayOfWeek == DayOfWeek.Sunday && dayName != "Sun") ||
                        (dt.DayOfWeek == DayOfWeek.Thursday && dayName != "Thu") ||
                        (dt.DayOfWeek == DayOfWeek.Tuesday && dayName != "Tue") ||
                        (dt.DayOfWeek == DayOfWeek.Wednesday && dayName != "Wed")
                        )
                        throw new FormatException("invalide week of day");
                }

                // adjust to localtime
                if (Regex.IsMatch(timeZone, "[+\\-][0-9][0-9][0-9][0-9]"))
                {
                    // it's a modern ANSI style timezone
                    int factor = 0;
                    hour = timeZone.Substring(1, 2);
                    minute = timeZone.Substring(3, 2);
                    if (timeZone.Substring(0, 1) == "+") factor = 1;
                    else if (timeZone.Substring(0, 1) == "-") factor = -1;
                    else throw new FormatException("incorrect tiem zone");
                    dt = dt.AddHours(factor * Convert.ToInt32(hour));
                    dt = dt.AddMinutes(factor * Convert.ToInt32(minute));
                }
                else
                {
                    // it's a old style military time zone ?
                    switch (timeZone)
                    {
                        case "A": dt = dt.AddHours(1); break;
                        case "B": dt = dt.AddHours(2); break;
                        case "C": dt = dt.AddHours(3); break;
                        case "D": dt = dt.AddHours(4); break;
                        case "E": dt = dt.AddHours(5); break;
                        case "F": dt = dt.AddHours(6); break;
                        case "G": dt = dt.AddHours(7); break;
                        case "H": dt = dt.AddHours(8); break;
                        case "I": dt = dt.AddHours(9); break;
                        case "K": dt = dt.AddHours(10); break;
                        case "L": dt = dt.AddHours(11); break;
                        case "M": dt = dt.AddHours(12); break;
                        case "N": dt = dt.AddHours(-1); break;
                        case "O": dt = dt.AddHours(-2); break;
                        case "P": dt = dt.AddHours(-3); break;
                        case "Q": dt = dt.AddHours(-4); break;
                        case "R": dt = dt.AddHours(-5); break;
                        case "S": dt = dt.AddHours(-6); break;
                        case "T": dt = dt.AddHours(-7); break;
                        case "U": dt = dt.AddHours(-8); break;
                        case "V": dt = dt.AddHours(-9); break;
                        case "W": dt = dt.AddHours(-10); break;
                        case "X": dt = dt.AddHours(-11); break;
                        case "Y": dt = dt.AddHours(-12); break;
                        case "Z":
                        case "UT":
                        case "GMT": break;    // It's UTC
                        case "EST": dt = dt.AddHours(5); break;
                        case "EDT": dt = dt.AddHours(4); break;
                        case "CST": dt = dt.AddHours(6); break;
                        case "CDT": dt = dt.AddHours(5); break;
                        case "MST": dt = dt.AddHours(7); break;
                        case "MDT": dt = dt.AddHours(6); break;
                        case "PST": dt = dt.AddHours(8); break;
                        case "PDT": dt = dt.AddHours(7); break;
                        default: throw new FormatException("invalide time zone");
                    }
                }
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format("Invalide date:{0}:{1}", e.Message, adate));
            }
            return dt;
        }
        #endregion
        #endregion
    }
    /// <summary>
    /// IParseableObject interface is used by <see cref="SyndicationObjectParser"/> and enables the class to supress events during the parsing operation.
    /// </summary>
    public interface IParseableObject
    {
        /// <summary>
        /// Called by <see cref="SyndicationObjectParser"/> before the parsing operation starts.
        /// </summary>
        void BeginParse();
        /// <summary>
        /// Called by <see cref="SyndicationObjectParser"/> after the parsing operation took place.
        /// </summary>
        void EndParse();
        /// <summary>
        /// Key: Type.Propertyname Value: Parser exception message
        /// </summary>
        System.Collections.Generic.SortedList<string, Exception> Errors { get;}
    }
    public interface IExtendableObject
    {
        System.Xml.Serialization.XmlSerializerNamespaces Xmlns { get;}
        System.Xml.XmlDocumentFragment ExtendedContent { get;}
    }
}
