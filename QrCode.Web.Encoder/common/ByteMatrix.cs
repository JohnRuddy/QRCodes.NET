using System;

namespace QrCode.Web.Encoder.common
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public sealed class ByteMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly sbyte[,] m_Bytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteMatrix"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <remarks></remarks>
        internal ByteMatrix(int width, int height)
        {
            m_Bytes = new sbyte[height,width];
        }

        /// <summary>
        /// Gets or sets the <see cref="System.SByte"/> with the specified x.
        /// </summary>
        /// <remarks></remarks>
        internal sbyte this[int x, int y]
        {
            get { return m_Bytes[y, x]; }
            set { m_Bytes[y, x] = value; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        internal int Width
        {
            get { return m_Bytes.GetLength(1); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        internal int Height
        {
            get { return m_Bytes.GetLength(0); }
        }

        /// <summary>
        /// Clears the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        internal void Clear(sbyte value)
        {
            ForAll((x, y, matrix) => { matrix[x, y] = value; });
        }

        /// <summary>
        /// Fors all.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        internal void ForAll(Action<int, int, ByteMatrix> action)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    action.Invoke(x, y, this);
                }
            }
        }
    }
}