using System.Collections;
using System.Collections.Generic;

namespace QrCode.Web.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal struct MatrixRectangle : IEnumerable<MatrixPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixRectangle"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <remarks></remarks>
        internal MatrixRectangle(MatrixPoint location, MatrixSize size) :
            this()
        {
            Location = location;
            Size = size;
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        /// <remarks></remarks>
        public MatrixPoint Location { get; private set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        /// <remarks></remarks>
        public MatrixSize Size { get; private set; }

        #region IEnumerable<MatrixPoint> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        /// <remarks></remarks>
        public IEnumerator<MatrixPoint> GetEnumerator()
        {
            for (int j = Location.Y; j < Location.Y + Size.Height; j++)
            {
                for (int i = Location.X; i < Location.X + Size.Width; i++)
                {
                    yield return new MatrixPoint(i, j);
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return string.Format("Rectangle({0};{1}):({2} x {3})", Location.X, Location.Y, Size.Width, Size.Height);
        }
    }
}