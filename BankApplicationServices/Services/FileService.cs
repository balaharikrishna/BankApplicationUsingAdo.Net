using BankApplicationModels;
using BankApplicationServices.IServices;
using System.Text.Json;

namespace BankApplicationServices.Services
{
    public class FileService : IFileService
    {
        private static string CheckFile()
        {
            string basePath = Path.GetTempPath();
            string fileName = "BankDetails";
            string fileExtension = ".json";

            string filePath = Path.ChangeExtension(Path.Combine(basePath, fileName), fileExtension);
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return filePath;
            }
            else
            {
                return filePath;
            }
        }
        public string ReadFile()
        {
            string jsonData = string.Empty;
            if (CheckFile() is not null)
            {
                jsonData = File.ReadAllText(CheckFile());
            }
            return jsonData;
        }

        public void WriteFile(List<Bank> banks)
        {
            string createBankJson = JsonSerializer.Serialize(banks);
            File.WriteAllText(CheckFile(), createBankJson);
            GetData();
        }

        public List<Bank> GetData()
        {
            List<Bank> data;
            if (ReadFile() is not null && !string.IsNullOrEmpty(ReadFile()))
            {
                data = JsonSerializer.Deserialize<List<Bank>>(ReadFile()) ?? new List<Bank>();
            }
            else
            {
                data = new List<Bank>();
                WriteFile(data);
                data = JsonSerializer.Deserialize<List<Bank>>(ReadFile()) ?? new List<Bank>();
            }
            return data;
        }
    }
}
