namespace BrainBox.Data.DTOs
{
    public class APIResponse<T>
    {
        /// <summary>
        /// Does the response have any errors.
        /// <para>If the response has any error the bool value is true.</para>
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// This contains the response object.
        /// <para>Return object is the result of the request</para>
        /// </summary>
        public T ResponseObject { get; set; }
    }
}
