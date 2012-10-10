using System;
using System.Collections;
using System.Collections.Generic;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal sealed class BitList : IEnumerable<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<byte> m_Array;

        /// <summary>
        /// 
        /// </summary>
        private int m_BitsSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        internal BitList()
        {
            m_BitsSize = 0;
            m_Array = new List<byte>(32);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <remarks></remarks>
        internal List<byte> List
        {
            get { return m_Array; }
        }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <remarks></remarks>
        internal bool this[int index]
        {
            get
            {
                if (index < 0 || index >= m_BitsSize)
                    throw new ArgumentOutOfRangeException("index", index, "Index out of range");
                int value_Renamed = m_Array[index >> 3] & 0xff;
                return ((value_Renamed >> (7 - (index & 0x7))) & 1) == 1;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <remarks></remarks>
        internal int Count
        {
            get { return m_BitsSize; }
        }

        /// <summary>
        /// Gets the size in byte.
        /// </summary>
        /// <remarks></remarks>
        internal int SizeInByte
        {
            get { return (m_BitsSize + 7) >> 3; }
        }

        #region IEnumerable<bool> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        /// <remarks></remarks>
        public IEnumerator<bool> GetEnumerator()
        {
            int numBytes = m_BitsSize >> 3;
            int remainder = m_BitsSize & 0x7;
            byte value;
            for (int index = 0; index < numBytes; index++)
            {
                value = m_Array[index];
                for (int shiftNum = 7; shiftNum >= 0; shiftNum--)
                {
                    yield return ((value >> shiftNum) & 1) == 1;
                }
            }
            if (remainder > 0)
            {
                value = m_Array[numBytes];
                for (int index = 0; index < remainder; index++)
                {
                    yield return ((value >> (7 - index)) & 1) == 1;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        /// <remarks></remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Toes the bit.
        /// </summary>
        /// <param name="item">if set to <c>true</c> [item].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int ToBit(bool item)
        {
            return item ? 1 : 0;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">if set to <c>true</c> [item].</param>
        /// <remarks></remarks>
        internal void Add(bool item)
        {
            int numBitsinLastByte = m_BitsSize & 0x7;
            //Add one more byte to List when we have no bits in the last byte. 
            if (numBitsinLastByte == 0)
                m_Array.Add(0);

            m_Array[m_BitsSize >> 3] |= (byte) (ToBit(item) << (7 - numBitsinLastByte));
            m_BitsSize++;
        }

        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <remarks></remarks>
        internal void Add(IEnumerable<bool> items)
        {
            foreach (bool item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitCount">The bit count.</param>
        /// <remarks></remarks>
        internal void Add(int value, int bitCount)
        {
            if (bitCount < 0 || bitCount > 32)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, "bitCount must greater or equal to 0");
            int numBitsLeft = bitCount;

            while (numBitsLeft > 0)
            {
                if ((m_BitsSize & 0x7) == 0 && numBitsLeft >= 8)
                {
                    //Add one more byte to List. 
                    var newByte = (byte) ((value >> (numBitsLeft - 8)) & 0xFF);
                    appendByte(newByte);
                    numBitsLeft -= 8;
                }
                else
                {
                    bool bit = ((value >> (numBitsLeft - 1)) & 1) == 1;
                    Add(bit);
                    numBitsLeft--;
                }
            }
        }

        /// <summary>
        /// Appends the byte.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <remarks></remarks>
        private void appendByte(byte item)
        {
            m_Array.Add(item);
            m_BitsSize += 8;
        }
    }
}