//dexter shell extension exercise


/********************************** Module Header **********************************\
Module Name:  FileContextMenuExt.cs
Project:      CSShellExtContextMenuHandler
Copyright (c) Microsoft Corporation.

The FileContextMenuExt.cs file defines a context menu handler by implementing the 
IShellExtInit and IContextMenu interfaces.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

#region Using directives
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
//using CSShellExtContextMenuHandler.Properties;
//using System.Drawing;
using System.Collections.Generic;
using System.IO;
//using System.Collections.Specialized;
using System.Linq;


#endregion


///http://www.codeproject.com/Articles/174369/How-to-Write-Windows-Shell-Extension-with-NET-Lang?msg=4466755#xx4466755xx
///http://msdn.microsoft.com/en-us/library/bb776881.aspx
namespace CSShellExtContextMenuHandler
{

    [ClassInterface(ClassInterfaceType.None)]
    //[Guid("B1F1405D-94A1-4692-B72F-FC8CAF8B8700"), ComVisible(true)]
    [Guid("4F09060A-3FD1-4AB1-8EEA-870FDEE3334D"), ComVisible(true)]
    public class FileContextMenuExt : IShellExtInit, IContextMenu
    {
        // The name of the selected file.
        //private string selectedFile;

        private string menuText = "New Folder with Selection";
        private IntPtr menuBmp = IntPtr.Zero;
        //private string verb = "csdisplay";
        //private string verbCanonicalName = "CSDisplayFileName";
        //private string verbHelpText = "Display File Name (C#)";
        private uint IDM_DISPLAY = 0;
        //private StringCollection selectedFiles = new StringCollection();
        private List<string> selectedFiles = new List<string>();

        private uint itemCount = 0;
        private Dictionary<int, string> pathMap = new Dictionary<int, string>();


        #region Shell Extension Registration

        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ShellExtReg.RegisterShellExtContextMenuHandler(t.GUID, "AllFileSystemObjects",
                    "New Folder with Selection APP");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ShellExtReg.UnregisterShellExtContextMenuHandler(t.GUID, "AllFileSystemObjects");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        #endregion


        #region IShellExtInit Members

        /// <summary>
        /// Initialize the context menu handler.
        /// </summary>
        /// <param name="pidlFolder">
        /// A pointer to an ITEMIDLIST structure that uniquely identifies a folder.
        /// </param>
        /// <param name="pDataObj">
        /// A pointer to an IDataObject interface object that can be used to retrieve 
        /// the objects being acted upon.
        /// </param>
        /// <param name="hKeyProgID">
        /// The registry key for the file object or folder type.
        /// </param>
        public void Initialize(IntPtr pidlFolder, IntPtr pDataObj, IntPtr hKeyProgID)
        {
            
            if (pDataObj == IntPtr.Zero)
            {
                throw new ArgumentException();
            }

            FORMATETC fe = new FORMATETC();
            fe.cfFormat = (short)CLIPFORMAT.CF_HDROP;
            fe.ptd = IntPtr.Zero;
            fe.dwAspect = DVASPECT.DVASPECT_CONTENT;
            fe.lindex = -1;
            fe.tymed = TYMED.TYMED_HGLOBAL;
            STGMEDIUM stm = new STGMEDIUM();

            // The pDataObj pointer contains the objects being acted upon. In this 
            // example, we get an HDROP handle for enumerating the selected files 
            // and folders.
            IDataObject dataObject = (IDataObject)Marshal.GetObjectForIUnknown(pDataObj);
            dataObject.GetData(ref fe, out stm);

            try
            {
                // Get an HDROP handle.
                IntPtr hDrop = stm.unionmember;
                if (hDrop == IntPtr.Zero)
                {
                    throw new ArgumentException();
                }

                // Determine how many files are involved in this operation.
                uint nFiles = NativeMethods.DragQueryFile(hDrop, UInt32.MaxValue, null, 0);
                //System.Windows.Forms.MessageBox.Show("kero_init, "+ nFiles.ToString()+"selected");
                // Enumerate the selected files and folders.
                if (nFiles > 0)
                {
                    
                    StringBuilder fileName = new StringBuilder(260);
                    for (uint i = 0; i < nFiles; i++)
                    {
                        // Get the next file name.
                        if (0 != NativeMethods.DragQueryFile(hDrop, i, fileName, fileName.Capacity))
                        {
                            // Add the file name to the list.
                            selectedFiles.Add(fileName.ToString());
                            //System.Windows.Forms.MessageBox.Show(fileName.ToString());
                        }
                    }
                    
                    // If we did not find any files we can work with, throw 
                    // exception.
                    if (selectedFiles.Count == 0)
                    {
                        Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                    }
                }
                else
                {
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }
            finally
            {
                NativeMethods.ReleaseStgMedium(ref stm);
            }
        }

        #endregion


        #region IContextMenu Members

        /// <summary>
        /// Add commands to a shortcut menu.
        /// </summary>
        /// <param name="hMenu">A handle to the shortcut menu.</param>
        /// <param name="iMenu">
        /// The zero-based position at which to insert the first new menu item.
        /// </param>
        /// <param name="idCmdFirst">
        /// The minimum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="idCmdLast">
        /// The maximum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="uFlags">
        /// Optional flags that specify how the shortcut menu can be changed.
        /// </param>
        /// <returns>
        /// If successful, returns an HRESULT value that has its severity value set 
        /// to SEVERITY_SUCCESS and its code value set to the offset of the largest 
        /// command identifier that was assigned, plus one.
        /// </returns>
        public int QueryContextMenu(
            IntPtr hMenu,
            uint iMenu,
            uint idCmdFirst,
            uint idCmdLast,
            uint uFlags)
        {
            //System.Windows.Forms.MessageBox.Show("kero_query");
            // If uFlags include CMF_DEFAULTONLY then we should not do anything.
            if (((uint)CMF.CMF_DEFAULTONLY & uFlags) != 0)
            {
                return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0);
            }


            IntPtr hSubMenu = NativeMethods.CreatePopupMenu();

            // PARENT!!!
            MENUITEMINFO mii2 = new MENUITEMINFO();
            mii2.cbSize = (uint)Marshal.SizeOf(mii2);
            mii2.fMask = /*MIIM.MIIM_BITMAP | MIIM.MIIM_SUBMENU | */MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_ID | MIIM.MIIM_STATE;
            //mii2.hSubMenu = hSubMenu;
            mii2.wID = idCmdFirst + itemCount++;
            mii2.fType = MFT.MFT_STRING;
            mii2.dwTypeData = this.menuText + " (" + this.selectedFiles.Count.ToString() + " Items)";
            if (this.selectedFiles.Count == 1)
            {
                mii2.dwTypeData = this.menuText + " (1 Item)";
                return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0);//there is no need to merge 1 folder...
            }
            if (this.selectedFiles.Count == 16)
            {
                mii2.dwTypeData = this.menuText + " (16 or More Items)";
            }

            
            mii2.fState = MFS.MFS_ENABLED;

            // Adding the POPUP Menu
            if (!NativeMethods.InsertMenuItem(hMenu, iMenu + 0, true, ref mii2))
            {
                return Marshal.GetHRForLastWin32Error();
            }

            return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0,
                IDM_DISPLAY + itemCount);
        }

        /// <summary>
        /// Carry out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">
        /// A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure 
        /// containing information about the command. 
        /// </param>
        public void InvokeCommand(IntPtr pici)
        {
            int lowerbound = 3;
            //System.Windows.Forms.MessageBox.Show("InvokeCommand");
            string lcs = commomFileName(this.selectedFiles);
            if (lcs.Length < lowerbound)
                lcs = "New Folder";
            
            string p = Path.GetDirectoryName(this.selectedFiles[0]);
            string fullpathname = p + "\\" + lcs;
            if (Directory.Exists(fullpathname))
                fullpathname = fullpathname + Path.GetRandomFileName().Substring(0,3);
            Directory.CreateDirectory(fullpathname);
            //string filelists = "";
            //move files into lcs
            //int i = 1;
            foreach (string fname in this.selectedFiles)
            {
                string dest = fullpathname + "\\" + Path.GetFileName(fname);
                
                if (File.Exists(fname))
                {
                    //System.Windows.Forms.MessageBox.Show("File:"+fname);
                    File.Move(fname, dest);
                }
                if (Directory.Exists(fname))
                {
                    //System.Windows.Forms.MessageBox.Show("Dir:" + fname);
                    //System.Windows.Forms.MessageBox.Show("Dir:" + dest);
                    Directory.Move(fname, dest);
                }

            }
            //System.Windows.Forms.MessageBox.Show(filelists);
        }
        public string commomFileName(List<string> fullfilenames)
        {
            //strip path
            List<string> filenames = new List<string>();
            foreach (string s in fullfilenames)
            {
                filenames.Add(Path.GetFileNameWithoutExtension(s));
            }


            Dictionary<string, int> term_freq = new Dictionary<string, int>();

            foreach (string filename in filenames)
            {
                for(int i=1;i<=filename.Length;i++)
                {
                    string substr = filename.Substring(0, i);

                    if (term_freq.Keys.Contains<string>(substr))
                        term_freq[substr]++;
                    else
                        term_freq.Add(substr, 1);
                }
            }
            int best_score = 0;
            string bestString = string.Empty;
            foreach (KeyValuePair<string, int> entry in term_freq)
            {
                int score = entry.Value * entry.Key.Length;
                if (score >= best_score)
                {
                    best_score = score;
                    bestString = entry.Key;
                }
            }
            
            return bestString.Trim();
        }
        
        public void GetCommandString(
                    UIntPtr idCmd,
                    uint uFlags,
                    IntPtr pReserved,
                    StringBuilder pszName,
                    uint cchMax)
        {
            return;
        }

        #endregion
    }
}