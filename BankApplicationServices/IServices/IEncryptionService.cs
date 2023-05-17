namespace BankApplicationServices.IServices
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Generates a random salt value.
        /// </summary>
        /// <returns>An array of bytes representing the generated salt value.</returns>
        byte[] GenerateSalt();

        /// <summary>
        /// Hashes a given password using a salt value.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <param name="salt">The salt value to be used in the password hashing process.</param>
        /// <returns>An array of bytes representing the resulting hash value.</returns>
        byte[] HashPassword(string password, byte[] salt);
    }
}