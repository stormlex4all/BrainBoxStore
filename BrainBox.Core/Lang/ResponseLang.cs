namespace BrainBox.Core.Lang
{
    /// <summary>
    /// Contains various reponse formats used
    /// </summary>
    public class ResponseLang
    {

        /// <summary>
        /// Sorry something went wrong while processing your request. Please try again later or contact admin
        /// </summary>
        public static string Genericexception(string param = "")
        {
            return string.Format("Sorry something went wrong while processing your request. Please try again later or contact admin.", param);
        }

        public static string UserAlreadyExists(string email)
        {
            return string.Format($"User {email} already exists");
        }

        public static string UserDoesNotExist(string email)
        {
            return string.Format($"User {email} does not exist");
        }

        public static string WrongUserCredential()
        {
            return string.Format($"Wrong user credentials");
        }

        public static string SimilarRecordExists(string recordName)
        {
            return string.Format($"A similar {recordName} record already Exists!");
        }

        public static string CouldNotCreateRecord(string recordName)
        {
            return string.Format($"Could not create {recordName} record!");
        }

        public static string CouldNotDeleteRecord()
        {
            return string.Format($"Could not create record!");
        }

        public static string RecordNotFound()
        {
            return string.Format($"Record was not found!");
        }

        public static string SuccessfullyUpdated()
        {
            return string.Format($"Record was updated successfully!");
        }

        public static string NotUpdated()
        {
            return string.Format($"Record was not updated successfully!");
        }

        public static string SuccessfullyDeleted()
        {
            return string.Format($"Record was deleted successfully!");
        }

        public static string NotDeleted()
        {
            return string.Format($"Record was not deleted successfully!");
        }
    }
}
