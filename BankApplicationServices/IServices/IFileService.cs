using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IFileService
    {
        /// <summary>
        /// Gets data of all Banks from the database.
        /// </summary>
        /// <returns>A List of Bank objects containing information about all Banks in the database.</returns>
        List<Bank> GetData();

        /// <summary>
        /// Reads data from a File.
        /// </summary>
        /// <returns>A string containing the contents of the file.</returns>
        string ReadFile();

        /// <summary>
        /// Writes the data of a List of Bank objects to a file.
        /// </summary>
        /// <param name="banks">A List of Bank objects containing the data to be written to the file.</param>
        void WriteFile(List<Bank> banks);
    }
}