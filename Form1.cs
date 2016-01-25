using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Net.Sockets;
using System.Net;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            load();
        }

        /// <summary>
        /// This method is used to copied all files and directory source to destionation
        /// manually
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// This method is used create directory and copiced all file and sub directory source to
        /// destination using zip
        /// </summary>
        public void load()
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(@"D:\Test");
            DirectoryInfo targetDirectory = new DirectoryInfo(@"D:\targetDirectory");

            //CopyAll(sourceDirectory, targetDirectory);
                        
            string startPath = @"D:\Test";
            string zipPath = @"D:\Test.zip";

            //string extractPath = @"D:\extract";
            
            ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, false);
            //ZipFile.ExtractToDirectory(zipPath, extractPath);

            //WebClient client = new WebClient();
            //NetworkCredential nc = new NetworkCredential("manikandan.palani", "Dec@2015");

            IPAddress[] ipAddress = Dns.GetHostAddresses("ISCHDESKAC15070");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress[1], 5055);
            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);


            byte[] fileNameByte = Encoding.ASCII.GetBytes("Test.zip");

            byte[] fileData = File.ReadAllBytes(@"D:\Test.zip");
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);

            clientSock.Bind(ipEnd);
            clientSock.Connect(ipEnd);
            clientSock.Send(clientData);
            clientSock.Close();

        }
    }
}
