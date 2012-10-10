using System;
using System.Collections.Generic;

namespace QrCode.Web.Encoder.DataEncodation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public sealed class ECISet
    {
        #region AppendOption enum

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        public enum AppendOption
        {
            /// <summary>
            /// 
            /// </summary>
            NameToValue,

            /// <summary>
            /// 
            /// </summary>
            ValueToName,

            /// <summary>
            /// 
            /// </summary>
            Both
        }

        #endregion

        /// <summary>
        /// ISO/IEC 18004:2006 Chapter 6.4.2 Mode indicator = 0111 Page 23
        /// </summary>
        private const int ECIMode = 7;

        /// <summary>
        /// 
        /// </summary>
        private const int ECIIndicatorNumBits = 4;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> s_NameToValue;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, string> s_ValueToName;

        /// <summary>
        /// Initialize ECI Set.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <remarks></remarks>
        internal ECISet(AppendOption option)
        {
            Initialize(option);
        }

        /// <summary>
        /// Appends the ECI.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="option">The option.</param>
        /// <remarks></remarks>
        private void AppendECI(string name, int value, AppendOption option)
        {
            switch (option)
            {
                case AppendOption.NameToValue:
                    s_NameToValue.Add(name, value);
                    break;
                case AppendOption.ValueToName:
                    s_ValueToName.Add(value, name);
                    break;
                case AppendOption.Both:
                    s_NameToValue.Add(name, value);
                    s_ValueToName.Add(value, name);
                    break;
                default:
                    throw new InvalidOperationException("There is no such AppendOption");
            }
        }

        /// <summary>
        /// Initializes the specified option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <remarks></remarks>
        private void Initialize(AppendOption option)
        {
            switch (option)
            {
                case AppendOption.NameToValue:
                    s_NameToValue = new Dictionary<string, int>();
                    break;
                case AppendOption.ValueToName:
                    s_ValueToName = new Dictionary<int, string>();
                    break;
                case AppendOption.Both:
                    s_NameToValue = new Dictionary<string, int>();
                    s_ValueToName = new Dictionary<int, string>();
                    break;
                default:
                    throw new InvalidOperationException("There is no such AppendOption");
            }

            //ECI table. Source 01 URL: http://strokescribe.com/en/ECI.html
            //ECI table. Source 02 URL: http://lab.must.or.kr/Extended-Channel-Interpretations-ECI-Encoding.ashx
            //ToDo. Fill up remaining missing table.
            AppendECI("iso-8859-1", 1, option);
            AppendECI("IBM437", 2, option);
            //AppendECI("iso-8859-1", 3, option);	//ECI value 1 is default encoding. 
            AppendECI("iso-8859-2", 4, option);
            AppendECI("iso-8859-3", 5, option);
            AppendECI("iso-8859-4", 6, option);
            AppendECI("iso-8859-5", 7, option);
            AppendECI("iso-8859-6", 8, option);
            AppendECI("iso-8859-7", 9, option);
            AppendECI("iso-8859-8", 10, option);
            AppendECI("iso-8859-9", 11, option);
            AppendECI("windows-874", 13, option);
            AppendECI("iso-8859-13", 15, option);
            AppendECI("iso-8859-15", 17, option);
            AppendECI("shift_jis", 20, option);
            AppendECI("utf-8", 26, option);
        }

        /// <summary>
        /// Gets the name of the ECI value by.
        /// </summary>
        /// <param name="encodingName">Name of the encoding.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal int GetECIValueByName(string encodingName)
        {
            if (s_NameToValue == null)
                Initialize(AppendOption.NameToValue);
            int ECIValue;
            if (s_NameToValue.TryGetValue(encodingName, out ECIValue))
                return ECIValue;
            else
                throw new ArgumentOutOfRangeException(string.Format("ECI doesn't contain encoding: {0}", encodingName));
        }

        /// <summary>
        /// Gets the ECI name by value.
        /// </summary>
        /// <param name="ECIValue">The ECI value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal string GetECINameByValue(int ECIValue)
        {
            if (s_ValueToName == null)
                Initialize(AppendOption.ValueToName);
            string ECIName;
            if (s_ValueToName.TryGetValue(ECIValue, out ECIName))
                return ECIName;
            else
                throw new ArgumentOutOfRangeException(string.Format("ECI doesn't contain value: {0}", ECIValue));
        }

        /// <summary>
        /// Nums the of codewords.
        /// </summary>
        /// <param name="ECIValue">Range: 0 ~ 999999</param>
        /// <returns>Number of Codewords(Byte) for ECI Assignment Value</returns>
        /// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
        private static int NumOfCodewords(int ECIValue)
        {
            if (ECIValue >= 0 && ECIValue <= 127)
                return 1;
            else if (ECIValue > 127 && ECIValue <= 16383)
                return 2;
            else if (ECIValue > 16383 && ECIValue <= 999999)
                return 3;
            else
                throw new ArgumentOutOfRangeException("ECIValue should be in range: 0 to 999999");
        }

        /// <summary>
        /// Nums the of assignment bits.
        /// </summary>
        /// <param name="ECIValue">Range: 0 ~ 999999</param>
        /// <returns>Number of bits for ECI Assignment Value</returns>
        /// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
        private static int NumOfAssignmentBits(int ECIValue)
        {
            return NumOfCodewords(ECIValue)*8;
        }

        /// <summary>
        /// Nums the of ECI header bits.
        /// </summary>
        /// <param name="ECIValue">Range: 0 ~ 999999</param>
        /// <returns>Number of bits for ECI Header</returns>
        /// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
        internal static int NumOfECIHeaderBits(int ECIValue)
        {
            return NumOfAssignmentBits(ECIValue) + 4;
        }

        /// <summary>
        /// Gets the ECI table.
        /// </summary>
        /// <returns>ECI table in Dictionary collection</returns>
        /// <remarks></remarks>
        public Dictionary<string, int> GetECITable()
        {
            if (s_NameToValue == null)
                Initialize(AppendOption.NameToValue);

            return s_NameToValue;
        }

        /// <summary>
        /// Determines whether [contains ECI name] [the specified encoding name].
        /// </summary>
        /// <param name="encodingName">Name of the encoding.</param>
        /// <returns><c>true</c> if [contains ECI name] [the specified encoding name]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool ContainsECIName(string encodingName)
        {
            if (s_NameToValue == null)
                Initialize(AppendOption.NameToValue);
            return s_NameToValue.ContainsKey(encodingName);
        }

        /// <summary>
        /// Determines whether [contains ECI value] [the specified eci value].
        /// </summary>
        /// <param name="eciValue">The eci value.</param>
        /// <returns><c>true</c> if [contains ECI value] [the specified eci value]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool ContainsECIValue(int eciValue)
        {
            if (s_ValueToName == null)
                Initialize(AppendOption.ValueToName);
            return s_ValueToName.ContainsKey(eciValue);
        }

        /// <summary>
        /// Gets the ECI header.
        /// </summary>
        /// <param name="encodingName">Name of the encoding.</param>
        /// <returns></returns>
        /// <remarks>ISO/IEC 18004:2006 Chapter 6.4.2 Page 24.</remarks>
        internal BitList GetECIHeader(string encodingName)
        {
            int eciValue = GetECIValueByName(encodingName);

            var dataBits = new BitList();

            dataBits.Add(ECIMode, ECIIndicatorNumBits);

            int eciAssignmentByte = NumOfCodewords(eciValue);
            //Number of bits = Num codewords indicator + codeword value = Number of codewords * 8
            //Chapter 6.4.2.1 ECI Designator ISOIEC 18004:2006 Page 24
            int eciAssignmentBits = 0;

            switch (eciAssignmentByte)
            {
                case 1:
                    //Indicator = 0. Page 24. Chapter 6.4.2.1
                    dataBits.Add((int) ECICodewordsLength.one, 1);
                    eciAssignmentBits = eciAssignmentByte*8 - 1;
                    break;
                case 2:
                    //Indicator = 10. Page 24. Chapter 6.4.2.1
                    dataBits.Add((int) ECICodewordsLength.two, 2);
                    eciAssignmentBits = eciAssignmentByte*8 - 2;
                    break;
                case 3:
                    //Indicator = 110. Page 24. Chapter 6.4.2.1
                    dataBits.Add((int) ECICodewordsLength.three, 3);
                    eciAssignmentBits = eciAssignmentByte*8 - 3;
                    break;
                default:
                    throw new InvalidOperationException("Assignment Codewords should be either 1, 2 or 3");
            }

            dataBits.Add(eciValue, eciAssignmentBits);

            return dataBits;
        }

        #region Nested type: ECICodewordsLength

        /// <summary>
        /// Length indicator for number of ECI codewords
        /// </summary>
        /// <description>Bits required for each one is:
        /// one = 1, two = 2, three = 3</description>
        /// <remarks>ISO/IEC 18004:2006 Chapter 6.4.2 Page 24.
        /// 1 codeword length = 0. Any additional codeword add 1 to front. Eg: 3 = 110</remarks>
        private enum ECICodewordsLength
        {
            /// <summary>
            /// 
            /// </summary>
            one = 0,

            /// <summary>
            /// 
            /// </summary>
            two = 2,

            /// <summary>
            /// 
            /// </summary>
            three = 6
        }

        #endregion
    }
}