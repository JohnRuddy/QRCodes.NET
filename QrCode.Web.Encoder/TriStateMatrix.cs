using System;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class TriStateMatrix : BitMatrix
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
        /// 
        /// </summary>
        private readonly StateMatrix m_stateMatrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriStateMatrix"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <remarks></remarks>
        public TriStateMatrix(int width)
        {
            m_stateMatrix = new StateMatrix(width);
            m_InternalArray = new bool[width,width];
            m_Width = width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriStateMatrix"/> class.
        /// </summary>
        /// <param name="internalArray">The internal array.</param>
        /// <remarks></remarks>
        internal TriStateMatrix(bool[,] internalArray)
        {
            m_InternalArray = internalArray;
            int width = internalArray.GetLength(0);
            m_stateMatrix = new StateMatrix(width);
            m_Width = width;
        }

        /// <summary>
        /// Return value will be deep copy of array.
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
            set
            {
                if (MStatus(i, j) == MatrixStatus.None || MStatus(i, j) == MatrixStatus.NoMask)
                {
                    throw new InvalidOperationException(
                        string.Format("The value of cell [{0},{1}] is not set or is Stencil.", i, j));
                }
                m_InternalArray[i, j] = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="System.Boolean"/> with the specified ERROR.
        /// </summary>
        /// <remarks></remarks>
        public bool this[int i, int j, MatrixStatus mstatus]
        {
            set
            {
                m_stateMatrix[i, j] = mstatus;
                m_InternalArray[i, j] = value;
            }
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
        /// Creates the tri state matrix.
        /// </summary>
        /// <param name="internalArray">The internal array.</param>
        /// <param name="triStateMatrix">The tri state matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CreateTriStateMatrix(bool[,] internalArray, out TriStateMatrix triStateMatrix)
        {
            triStateMatrix = null;
            if (internalArray == null)
                return false;

            if (internalArray.GetLength(0) == internalArray.GetLength(1))
            {
                triStateMatrix = new TriStateMatrix(internalArray);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Ms the status.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal MatrixStatus MStatus(int i, int j)
        {
            return m_stateMatrix[i, j];
        }

        /// <summary>
        /// Ms the status.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal MatrixStatus MStatus(MatrixPoint point)
        {
            return MStatus(point.X, point.Y);
        }
    }
}