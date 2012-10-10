namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class SquareBitMatrix : BitMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly bool[,] m_InternalArray;

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_Width;

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBitMatrix"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <remarks></remarks>
        public SquareBitMatrix(int width)
        {
            m_InternalArray = new bool[width,width];
            m_Width = width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBitMatrix"/> class.
        /// </summary>
        /// <param name="internalArray">The internal array.</param>
        /// <remarks></remarks>
        internal SquareBitMatrix(bool[,] internalArray)
        {
            m_InternalArray = internalArray;
            int width = internalArray.GetLength(0);
            m_Width = width;
        }

        /// <summary>
        /// Return value will be internal array itself. Not deep/shallow copy.
        /// </summary>
        /// <remarks></remarks>
        public override bool[,] InternalArray
        {
            get
            {
                var deepCopyArray = new bool[m_Width,m_Width];
                for (int x = 0; x < m_Width; x++)
                    for (int y = 0; y < m_Width; y++)
                        deepCopyArray[x, y] = m_InternalArray[x, y];
                return deepCopyArray;
            }
        }


        /// <summary>
        /// Gets or sets the <see cref="System.Boolean"/> with the specified i.
        /// </summary>
        /// <remarks></remarks>
        public override bool this[int i, int j]
        {
            get { return m_InternalArray[i, j]; }
            set { m_InternalArray[i, j] = value; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <remarks></remarks>
        public override int Height
        {
            get { return Width; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <remarks></remarks>
        public override int Width
        {
            get { return m_Width; }
        }

        /// <summary>
        /// Creates the square bit matrix.
        /// </summary>
        /// <param name="internalArray">The internal array.</param>
        /// <param name="triStateMatrix">The tri state matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CreateSquareBitMatrix(bool[,] internalArray, out SquareBitMatrix triStateMatrix)
        {
            triStateMatrix = null;
            if (internalArray == null)
                return false;

            if (internalArray.GetLength(0) == internalArray.GetLength(1))
            {
                triStateMatrix = new SquareBitMatrix(internalArray);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}