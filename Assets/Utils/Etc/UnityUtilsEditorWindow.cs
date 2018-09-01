#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using Ionic.Zip;

public class UnityUtilsEditorWindow : EditorWindow {

    static string currentVersion = "NULLREF";
    static string gitVersion = "NULLREF";

    static Texture2D githubLogo;

    //Add it
    [MenuItem("Help/Unity Utils")]
    static void Init()
    {
        //Update the current version.
        dataPath = Application.dataPath;
        currentVersion = getCurrentVersion();
        setGitVersion();
        githubLogo = getGithubLogo();
        //Called when the windows is opened, aka the button is pressed.
        var window = (UnityUtilsEditorWindow)EditorWindow.GetWindow(typeof(UnityUtilsEditorWindow));
        window.titleContent = new GUIContent("Unity Utils Git");
        window.Show();
    }

    static string pathToEtcDirectory()
    {
        string[] files = Directory.GetFiles(dataPath, "*.cs", SearchOption.AllDirectories);
        foreach(string file in files)
        {
            if(Path.GetFileName(file) == "UnityUtilsEditorWindow.cs")
            {
                //This is our file.
                return Path.GetDirectoryName(file) + "\\";
            }
        }
        return "Couldn't find it.";
    }
    static string pathToModulesDirectory()
    {
        string etc = pathToEtcDirectory();
        return Path.GetFullPath(Path.Combine(etc, @"..\Modules\"));
    }

    static string dataPath;

    static string getCurrentVersion()
    {
        //Open the file
        string versionFilePath = pathToEtcDirectory() + "version.txt";
        var fStream = new StreamReader(versionFilePath);
        return fStream.ReadToEnd();
    }
    static Texture2D getGithubLogo()
    {
        Texture2D t = new Texture2D(2,2);
        t.LoadImage(File.ReadAllBytes(pathToEtcDirectory() + "GitHub_Logo.png"));
        return t;
    }
    static void setGitVersion()
    {
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

        string siteURL = @"https://raw.githubusercontent.com/DeathGameDev/Unity-Utils/master/Assets/Utils/Etc/version.txt";
        System.Net.WebClient wc = new System.Net.WebClient();
        gitVersion = "Pinging...";
        //Download the file to a temp dir
        wc.DownloadStringAsync(new System.Uri(siteURL));
        wc.DownloadStringCompleted += (sender, args) =>
        {
            gitVersion = args.Result;
            wc.Dispose();
        };
    }

    static bool isDownloading = false;
    static int downloadProgress = 0;
    static bool isExtracting = false;

    static void UpdateCode()
    {
        //Download the zip file.
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

        isDownloading = true;
        WebClient wc = new WebClient();
        wc.DownloadFileAsync(new System.Uri("https://github.com/DeathGameDev/Unity-Utils/archive/master.zip"), "temp.zip");
        wc.DownloadFileCompleted += (sender, args) =>
        {
            isDownloading = false;
            Debug.Log("File completed");
            //Now that we have the file. Unzip it into a temp dir.
            if (Directory.Exists("temp_dl"))
                Directory.Delete("temp_dl", true);
            Directory.CreateDirectory("temp_dl");
            isExtracting = true;
            using (ZipFile zip = ZipFile.Read("temp.zip"))
            {
                zip.ExtractAll("temp_dl");
            }
            
            // Now that we've extracted it, delete the old modules folder.
            string folderToDelete = pathToModulesDirectory();
            if(Directory.Exists(folderToDelete))
                Directory.Delete(folderToDelete, true);
            
            // Import the new modules folder to the same path.
            Directory.CreateDirectory(pathToModulesDirectory());
            string goalPath = pathToModulesDirectory();
            string folderToCopy = "temp_dl\\Unity-Utils-master\\Assets\\Utils\\Modules\\";
            Debug.Log("Copying the dir. From " + folderToCopy + " to " + goalPath);
            DirectoryCopy(folderToCopy, goalPath, true);

            // Also copy over our changelog.txt and version.txt
            File.Copy("temp_dl\\Unity-Utils-master\\Assets\\Utils\\Etc\\version.txt", pathToEtcDirectory() + "version.txt", true);
            File.Copy("temp_dl\\Unity-Utils-master\\Assets\\Utils\\Etc\\changelog.txt", pathToEtcDirectory() + "changelog.txt", true);

            // Now set our extracting state to false, and delete the temporary files.
            isExtracting = false;

            Directory.Delete("temp_dl", true);
            File.Delete("temp.zip");
        };
        wc.DownloadProgressChanged += (sender, args) =>
        {
            downloadProgress = args.ProgressPercentage;
        };
    }

    void Update() { Repaint(); }

    float indeterminateProgress = 0.0f;
    bool indeterminateAdding = true;

    void OnGUI()
    {
        //Indeterminite stuff
        if (isExtracting)
        {
            if (indeterminateProgress > 1)
                indeterminateAdding = false;
            if (indeterminateProgress < 0)
                indeterminateAdding = true;
            if (indeterminateAdding)
                indeterminateProgress += Time.deltaTime * 10 / 100;
            else
                indeterminateProgress -= Time.deltaTime * 10 / 100;
        }

        if (isDownloading)
            EditorUtility.DisplayProgressBar("Unity Utils - Git Update", "Downloading new files from the Git Repository", downloadProgress / 100f);
        else if(!isExtracting)
            EditorUtility.ClearProgressBar();

        if (isExtracting)
            EditorUtility.DisplayProgressBar("Unity Utils - Git Update", "Extracting and applying the new files.", indeterminateProgress);
        else if (!isDownloading)
            EditorUtility.ClearProgressBar();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Local Ver: " + currentVersion);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Remote Ver: " + gitVersion);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (currentVersion != gitVersion && gitVersion != "Pinging...") {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("New version detected!");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Update"))
            {
                //Update
                UpdateCode();
            }
        }else
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No new version detected.");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();

        if (githubLogo != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(githubLogo, GUILayout.MaxWidth(128), GUILayout.MaxHeight(128));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("@DeathGameDev"))
        {
            Application.OpenURL("http://twitter.com/DeathGameDev");
        }

        GUILayout.FlexibleSpace();

        GUILayout.Label("© 2016 GitHub, Inc.");

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    //Thank you, StackOverflow and MSDN.
    private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        //Return true if the server certificate is ok
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        bool acceptCertificate = true;
        string msg = "The server could not be validated for the following reason(s):\r\n";

        //The server did not present a certificate
        if ((sslPolicyErrors &
             SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable)
        {
            msg = msg + "\r\n    -The server did not present a certificate.\r\n";
            acceptCertificate = false;
        }
        else
        {
            //The certificate does not match the server name
            if ((sslPolicyErrors &
                 SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                msg = msg + "\r\n    -The certificate name does not match the authenticated name.\r\n";
                acceptCertificate = false;
            }

            //There is some other problem with the certificate
            if ((sslPolicyErrors &
                 SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
            {
                foreach (X509ChainStatus item in chain.ChainStatus)
                {
                    if (item.Status != X509ChainStatusFlags.RevocationStatusUnknown &&
                        item.Status != X509ChainStatusFlags.OfflineRevocation)
                        break;

                    if (item.Status != X509ChainStatusFlags.NoError)
                    {
                        msg = msg + "\r\n    -" + item.StatusInformation;
                        acceptCertificate = false;
                    }
                }
            }
        }

        //If Validation failed, present message box
        if (acceptCertificate == false)
        {
            msg = msg + "\r\nDo you wish to override the security check?";
            //          if (MessageBox.Show(msg, "Security Alert: Server could not be validated",
            //                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            acceptCertificate = true;
        }

        return acceptCertificate;
    }
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}
#endif