using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace DiskInformation
{
    public partial class Form1 : Form
    {
        private bool IsInfoAvailiable = false;
        float Piemark = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ManagementObjectSearcher diskInfors = new ManagementObjectSearcher("Select * from Win32_Diskdrive");
            
            foreach(ManagementObject diskInfor in diskInfors.Get())
            {
                cmb_listDisk.Items.Add(diskInfor["Model"].ToString());
            }
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            for (int i = 0; i <= drives.Length - 1; i++)
            {
                cmbVolume.Items.Add(drives[i].Name);
            }
            cmb_listDisk.SelectedIndex = 0;
            cmbVolume.SelectedIndex = 0;
        }

        private void cmb_listVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManagementObjectSearcher diskInfors = new ManagementObjectSearcher("Select * from Win32_Diskdrive where Model = '" + cmb_listDisk.SelectedItem + "'");
            
            foreach (ManagementObject diskInfor in diskInfors.Get())
            {
                lbl_Type.Text = "Type: " + diskInfor["MediaType"].ToString();
                lbl_Model.Text = "Model: " + diskInfor["Model"].ToString();
                lbl_Serial.Text = "Serial: " + diskInfor["SerialNumber"].ToString();
                lbl_Interface.Text = "Interface: " + diskInfor["InterfaceType"].ToString();
                lbl_Capacity.Text = "Capacity: " + diskInfor["Size"].ToString() + " bytes (" + Math.Round(((((double)Convert.ToDouble(diskInfor["Size"]) / 1024) / 1024) / 1024), 2) + " GB)";
                lbl_Partitions.Text = "Partitions: " + diskInfor["Partitions"].ToString();
                lbl_Signature.Text = "Signature: " + diskInfor["Signature"].ToString();
                //lbl_Firmware.Text = "Firmware: " + diskInfor["FirmwareRevision"].ToString();
                lbl_Cylinders.Text = "Cylinders: " + diskInfor["TotalCylinders"].ToString();
                lbl_Sectors.Text = "Sectors: " + diskInfor["TotalSectors"].ToString();
                lbl_Heads.Text = "Heads: " + diskInfor["TotalHeads"].ToString();
                lbl_Tracks.Text = "Tracks: " + diskInfor["TotalTracks"].ToString();
                lbl_BytesPerSector.Text = "Bytes per Sector: " + diskInfor["BytesPerSector"].ToString();
                lbl_SectorsPerTrack.Text = "Sectors per Track: " + diskInfor["SectorsPerTrack"].ToString();
                lbl_TracksPerCylinder.Text = "Tracks per Cylinder: " + diskInfor["TracksPerCylinder"].ToString();   
            }
            
        }

        private void cmbVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.IO.DriveInfo Drive_Info = null;
            if (cmbVolume.SelectedIndex != -1)
            {
                Drive_Info = new System.IO.DriveInfo(cmbVolume.Text);
                lblName.Text = Drive_Info.Name;
                if (Drive_Info.IsReady)
                {
                    if (Drive_Info.VolumeLabel.Length > 0)
                    {
                        lblVolumeLabel.Text = Drive_Info.VolumeLabel;
                    }
                    else
                    {
                        lblVolumeLabel.Text = "None";
                    }
                    lblType.Text = Drive_Info.DriveFormat;
                    lblType.Text = Drive_Info.DriveType.ToString();
                    lblRootDir.Text = Drive_Info.RootDirectory.FullName.ToString() ;
                    lblCapacity.Text = Drive_Info.TotalSize + " (Bytes)";
                    lblAvalspace.Text = Drive_Info.TotalFreeSpace + " (Bytes)";
                    long usedSpace = 0;
                    usedSpace = Drive_Info.TotalSize - Drive_Info.TotalFreeSpace;
                    lblUsedSpace.Text = usedSpace + " (Bytes)";
                    //ProgressBar1.Value = 0
                    Piemark = 360f * Drive_Info.TotalFreeSpace / Drive_Info.TotalSize;
                    long ProgressCurrentValue = usedSpace * 100 / Drive_Info.TotalSize;
                    IsInfoAvailiable = true;
                    //ProgressBar1.Value = ProgressCurrentValue
                    //ProgressBar1.Show()
                }
                else
                {
                    IsInfoAvailiable = false;
                    lblType.Text = "";
                    lblType.Text = "";
                    lblRootDir.Text = "";
                    lblCapacity.Text = "";
                    label10.Text = "";
                    lblUsedSpace.Text = "";

                }
                groupBox1.Invalidate();
                if (IsInfoAvailiable == false)
                {
                    MessageBox.Show("Drive is not ready");
                }
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(35, 20, 110, 50);
            if (IsInfoAvailiable)
            {
                e.Graphics.FillPie(Brushes.Fuchsia, rect, 0, Piemark);
                e.Graphics.FillPie(Brushes.Blue, rect, Piemark, 360 - Piemark);
                Application.DoEvents();
            }
        }

        
    }
}
