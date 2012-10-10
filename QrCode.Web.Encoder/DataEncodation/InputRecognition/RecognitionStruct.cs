namespace QrCode.Web.Encoder.DataEncodation.InputRecognition
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public struct RecognitionStruct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecognitionStruct"/> struct.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="encodingName">Name of the encoding.</param>
        /// <remarks></remarks>
        public RecognitionStruct(Mode mode, string encodingName)
            : this()
        {
            Mode = mode;
            EncodingName = encodingName;
        }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        /// <remarks></remarks>
        public Mode Mode { get; private set; }

        /// <summary>
        /// Gets or sets the name of the encoding.
        /// </summary>
        /// <value>The name of the encoding.</value>
        /// <remarks></remarks>
        public string EncodingName { get; private set; }
    }
}