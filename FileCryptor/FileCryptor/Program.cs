using System.Security.Cryptography;

namespace FileCryptor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[ EncryptorX ] - [ File Encryption and Decryption App ]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] Encrypt File");
            Console.WriteLine("[2] Decrypt File");
            Console.Write("Choose an option : ");
            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Invalid selection!");
                return;
            }

            Console.Clear();

            switch (choice)
            {
                case 1:
                    EncryptFile();
                    break;
                case 2:
                    DecryptFile();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Invalid selection!");
                    break;
            }
        }

        static readonly string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
        static void EncryptFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[*] Enter the path of the file to encrypt");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(desktopPath + " : ");
            string inputFile = Console.ReadLine().Trim();
            Console.Clear();


            inputFile = desktopPath + inputFile;

            if (!File.Exists(inputFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] The specified file was not found!");
                return;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[*] Enter the encryption password: ");
            string password = Console.ReadLine();
            Console.Clear();

            string outputFile = desktopPath+Path.GetFileNameWithoutExtension(inputFile) + "_encrypted" + Path.GetExtension(inputFile);

            SecureFileEncryptor.EncryptFile(inputFile, outputFile, password);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"File successfully encrypted: {outputFile}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Do you want to delete the original file? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                File.Delete(inputFile);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Original file deleted.");
            }
        }

        static void DecryptFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[*] Enter the path of the file to decrypt");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(desktopPath + " : ");
            string inputFile = Console.ReadLine().Trim();

            inputFile = desktopPath + inputFile;

            if (!File.Exists(inputFile))
            {
                Console.WriteLine("[!] The specified file was not found!");
                return;
            }

            Console.Clear();

            Console.Write("[*] Enter the decryption password: ");
            string password = Console.ReadLine();
            Console.Clear();
            string outputFile = desktopPath+Path.GetFileNameWithoutExtension(inputFile) + "_decrypted" + Path.GetExtension(inputFile);

            SecureFileEncryptor.DecryptFile(inputFile, outputFile, password);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"File successfully decrypted: {outputFile}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Do you want to delete the encrypted file? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                File.Delete(inputFile);
                File.Delete(inputFile + ".iv"); // Delete the IV file as well
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Encrypted file deleted.");
            }


        }
    }
}
