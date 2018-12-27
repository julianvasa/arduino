using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


enum RecycleFlags : int
{
    // No confirmation dialog when emptying the recycle bin
    SHERB_NOCONFIRMATION = 0x00000001,
    // No progress tracking window during the emptying of the recycle bin
    SHERB_NOPROGRESSUI = 0x00000001,
    // No sound whent the emptying of the recycle bin is complete
    SHERB_NOSOUND = 0x00000004
}


namespace pano
{
    public partial class Main : Form
    {
        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        String initPathFolder;
        String cam1;
        bool ndalo = false;
        int files = 0;
        int remaining = 0;
        Bitmap img;
        String panotoolsdirGlobal;
        String watermarkFilename;
        String ImageMagickPath;
        String trackFilename;
        String final;
        int threadcount = 0;
        Stopwatch sw = Stopwatch.StartNew();
        int nrThreads = 4;

        public Main()
        {
            InitializeComponent();


            if (albumi.Properties.Settings.Default.initDir.Length > 0)
            {
                this.initPathLabel.Text = albumi.Properties.Settings.Default.initDir;
                this.initPathFolder = this.initPathLabel.Text;
                this.cam1 = this.initPathFolder;
              
                TotFoto(this.cam1);
                totFotos.Text = files.ToString();
                remaining = files;
                fotombetura.Text = (remaining ).ToString();
                fotoperfunduara.Text = ((files - remaining)).ToString();
                fotoperfunduara.Refresh();               
            }
           
            if (albumi.Properties.Settings.Default.imageMagicPath.Length > 0)
            {
                this.imageMagickDir.Text = albumi.Properties.Settings.Default.imageMagicPath;
                this.ImageMagickPath = this.imageMagickDir.Text.Replace("\\", "\\\\");
            }
            if (albumi.Properties.Settings.Default.krpanotoolsPath.Length > 0)
            {
                this.panotoolsdir.Text = albumi.Properties.Settings.Default.krpanotoolsPath;
                this.panotoolsdirGlobal = this.panotoolsdir.Text.Replace("\\", "\\\\");
            }
            if (albumi.Properties.Settings.Default.watermark.Length > 0)
            {
                this.watermark.Text = albumi.Properties.Settings.Default.watermark;
                this.watermarkFilename = this.watermark.Text.Replace("\\", "\\\\");
            }
            if (albumi.Properties.Settings.Default.track.Length > 0)
            {
                this.track.Text = albumi.Properties.Settings.Default.track;
                this.trackFilename = this.track.Text.Replace("\\", "\\\\");
            }
                        
            initID.Text = albumi.Properties.Settings.Default.lastId.ToString();

        }

        private void folderPath_Click(object sender, EventArgs e)
        {
            if (zgjidhDirektori.ShowDialog() == DialogResult.OK)
            {
                this.initPathLabel.Text = zgjidhDirektori.SelectedPath;
                this.initPathFolder = zgjidhDirektori.SelectedPath;
                this.initPathLabel.Text = this.initPathFolder;
                this.cam1 = this.initPathFolder;
               
                TotFoto(this.cam1);
                totFotos.Text = (files).ToString();
                remaining = files;
                fotombetura.Text = (remaining).ToString();
                fotoperfunduara.Text = ((files - remaining)).ToString();
                fotoperfunduara.Refresh();

            }
        }

        public void TotFoto(string dir)
        {
            dir = dir + @"\";
            if (Directory.Exists(dir))
            {
                //String[] all_files = Directory.GetFileSystemEntries(dir);
                String[] all_files = Directory.GetFiles(dir, "*.tif").ToArray();
                foreach (string file in all_files)
                {
                    if (Directory.Exists(file))
                    {
                        TotFoto(file);
                    }
                    else
                    {
                        Console.WriteLine(Path.GetFileName(file));
                        files++;
                    }
                }
            }
        }
       
        public void MovePhotosThread(string foto)
        {
                if (File.Exists(foto))
                {
                    Console.WriteLine("Processing file: " + foto.ToString());
                    Directory.CreateDirectory(this.initPathFolder + "\\" + Path.GetFileNameWithoutExtension(foto));
                    File.Move(Path.GetFullPath(foto), this.initPathFolder + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileName(foto));
                    
                    /*
                    string[] tifs = Directory.GetFiles(this.panos, "*.tif").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();

                        int i = 0;
                        foreach (string f in tifs)
                        {
                            i++;
                            if (i > 1)
                            {
                                Console.WriteLine(f);
                                File.Delete(f);
                            }
                            else
                            {
                                Console.WriteLine(f);
                                if (File.Exists(f))
                                {
                                    File.Move(f, this.panos + "\\" + Path.GetFileNameWithoutExtension(f) + "\\" + Path.GetFileName(f));
                                }
                            }

                        }
                    */

                    status.Text = status.Text + "\r\nKrpano po e ben copa copa foton " + Path.GetFileNameWithoutExtension(foto);
                    Console.WriteLine(this.panotoolsdirGlobal + "\\krpanotools64.exe makepano -config=" + this.panotoolsdirGlobal + "\\templates\\multires.config " + this.initPathFolder + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileName(foto));

                    executeExe(this.panotoolsdirGlobal + "\\krpanotools64.exe ", " makepano -config=" + this.panotoolsdirGlobal + "\\templates\\multires.config " + this.initPathFolder + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileName(foto));
                    /*
                    if (!Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto))) Directory.CreateDirectory(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto));
                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg")) File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg");
                               
                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_b.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_b.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_b.jpg");
                    }
                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_d.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_d.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_d.jpg");
                    }
                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_l.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_l.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_l.jpg");
                    }
                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_r.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_r.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_r.jpg");
                    }

                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_u.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_u.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_u.jpg");
                    }

                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_f.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles\\tablet_f.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\tablet_f.jpg");
                    }


                    status.Text = status.Text + "\r\nImageMagick po e mbron foton " + Path.GetFileNameWithoutExtension(foto) + " nga hajdutet";
                    gotobottom();
                    WatermarkPhotos(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));

                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg")) File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg");

                    

                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg"))
                    {
                        File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.jpg", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\mobile\\preview.jpg");
                    }

                     * 
                     * */


                    /*
                    if (Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles"))
                    {
                        tifs = Directory.GetFiles(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles", "*");

                        foreach (string f in tifs)
                        {
                            File.Delete(f);
                        }
                    }
                     * */

                    /*
                    if (Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto))) Directory.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto));
                    if (Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles")) Directory.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tiles");
                    */
                    status.Text = status.Text + "\r\nTrack po ben llogaritje astronomike: " + Path.GetFileNameWithoutExtension(foto);
                    //trackCalc(Int32.Parse(Path.GetFileNameWithoutExtension(foto)));
                    status.Text = status.Text + "\r\nProgrami po ben pastrimin e shtepise: " + Path.GetFileNameWithoutExtension(foto);
                    status.Text = status.Text + "\r\nTrete koshin!!!!";
                    SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOSOUND | RecycleFlags.SHERB_NOCONFIRMATION);
                    gotobottom();
                    remaining = remaining - 5;
                    fotombetura.Text = (remaining / 5).ToString();
                    fotombetura.Refresh();
                    fotoperfunduara.Text = ((this.files - remaining) / 5).ToString();
                    fotoperfunduara.Refresh();
                    this.Refresh();
                    this.Update();
                    this.threadcount--;
                              
                }
           
        }

        public void WatermarkPhotos(string dir)
        {
         //   Console.WriteLine("WatermarkPhotos " + dir.ToString());
            if (Directory.Exists(dir))
            {
                dir = dir + @"\";
                String[] all_files = Directory.GetFiles(dir);
                
                foreach (string file in all_files)
                {
                    if (Path.GetExtension(file) == ".jpg" && Path.GetFileNameWithoutExtension(file) != "preview_mobile" && Path.GetFileNameWithoutExtension(file) != "thumb_big" && Path.GetFileNameWithoutExtension(file) != "thumb" && Path.GetFileNameWithoutExtension(file) != "preview")
                    {
                        executeExe(this.ImageMagickPath + "\\composite", " -dissolve 100% -gravity Center " + this.watermarkFilename + " " + file + " " + file);
                    }
                }

                foreach (string d in Directory.GetDirectories(dir))
                {
                    WatermarkPhotos(d);
                }                
            }
        }

      

        public void executeExe(string filename, string args)
        {
            Console.WriteLine("Executing: " + filename);
            Thread execthread = new Thread(() =>
            {
                System.Diagnostics.ProcessStartInfo proc;
                proc = new System.Diagnostics.ProcessStartInfo();
                proc.FileName = filename;
                proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.Arguments = args;
                proc.UseShellExecute = false;
                proc.CreateNoWindow = true;

                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(proc))
                {

                    process.WaitForExit(60000);
                    process.Close();    
                }

            });
            execthread.IsBackground = true;
            execthread.Start();

            while (execthread.IsAlive) Application.DoEvents();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }


        private void deleteFile(String dir)
        {
            if (Directory.Exists(dir))
            {
                string[] deletetmp = Directory.GetFiles(dir, "*");
                foreach (string f in deletetmp)
                {
                    File.Delete(f);
                }
                Directory.Delete(dir);
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            ndalo = false;
            folderPath.Enabled = false;
            initPathLabel.Enabled = false;
            imageMagickBtn.Enabled = false;
            imageMagickDir.Enabled = false;
            panotoolsBtn.Enabled = false;
            panotoolsdir.Enabled = false;
            watermark.Enabled = false;
            watermarkBtn.Enabled = false;
            track.Enabled = false;
            trackBtn.Enabled = false;
            initID.Enabled = false;
            start.Enabled = false;
           
            albumi.Properties.Settings.Default.initDir = this.initPathLabel.Text;
            albumi.Properties.Settings.Default.imageMagicPath = this.imageMagickDir.Text;
            albumi.Properties.Settings.Default.krpanotoolsPath = this.panotoolsdir.Text;
            albumi.Properties.Settings.Default.watermark = this.watermark.Text;
            albumi.Properties.Settings.Default.track = this.track.Text;
            albumi.Properties.Settings.Default.Save();

            if (!File.Exists(this.track.Text) || !File.Exists(this.watermark.Text) || !Directory.Exists(this.panotoolsdir.Text) || !Directory.Exists(this.imageMagickDir.Text) || !Directory.Exists(this.initPathLabel.Text))
            {
                MessageBox.Show("Nje ose me shume direktori ose file mungojne!");
                folderPath.Enabled = true;
                initPathLabel.Enabled = true;
                imageMagickBtn.Enabled = true;
                imageMagickDir.Enabled = true;
                panotoolsBtn.Enabled = true;
                panotoolsdir.Enabled = true;
                watermark.Enabled = true;
                watermarkBtn.Enabled = true;
                track.Enabled = true;
                trackBtn.Enabled = true;
                initID.Enabled = true;
                return;
            }
            
           // eraseTmp();

            /*
            if (Directory.Exists(this.panos))
            {
                string[] tmppics = Directory.GetFiles(this.panos, "*").ToArray();

                foreach (string f in tmppics)
                {
                    File.Delete(f);
                }
            }
            */
            string[] threads = Directory.GetFiles(this.cam1, "*.tif").ToArray();

             foreach (string f in threads)
             {
                 if (ndalo == false) MovePhotosThread(f);
                 else {
                     sw.Stop();
                     TimeSpan elapsedTime = sw.Elapsed;
                     string elapsedTimeFormated = String.Format("{0:00} dite {1:00} ore {2:00} min {3:00} sek", elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                     status.Text = status.Text + "\r\nKreu per: " + elapsedTimeFormated + " ne oren: " + DateTime.Now;
                     gotobottom();
                     folderPath.Enabled = true;
                     initPathLabel.Enabled = true;
                     imageMagickBtn.Enabled = true;
                     imageMagickDir.Enabled = true;
                     panotoolsBtn.Enabled = true;
                     panotoolsdir.Enabled = true;
                     watermark.Enabled = true;
                     watermarkBtn.Enabled = true;
                     track.Enabled = true;
                     trackBtn.Enabled = true;
                     initID.Enabled = true;
                     start.Enabled = true;
                     break;
                 }
            }
           
        }

        private void resume()
        {
            string[] threads = Directory.GetFiles(this.cam1, "*.JPG").ToArray();

            foreach (string f in threads)
            {
                    MovePhotosThread(f);
            }
          

            if (threadcount == 0)
            {
               
                     sw.Stop();
                     TimeSpan elapsedTime = sw.Elapsed;
                     string elapsedTimeFormated = String.Format("{0:00} dite {1:00} ore {2:00} min {3:00} sek", elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                     status.Text = status.Text + "\r\nKreu per: " + elapsedTimeFormated + " ne oren: " + DateTime.Now;
                     gotobottom();
                     folderPath.Enabled = true;
                     initPathLabel.Enabled = true;
                     imageMagickBtn.Enabled = true;
                     imageMagickDir.Enabled = true;
                     panotoolsBtn.Enabled = true;
                     panotoolsdir.Enabled = true;
                     watermark.Enabled = true;
                     watermarkBtn.Enabled = true;
                     track.Enabled = true;
                     trackBtn.Enabled = true;
                     initID.Enabled = true;
                     start.Enabled = true;
            }
        }

        private void panotoolsBtn_Click(object sender, EventArgs e)
        {
            if (zgjidhDirektori.ShowDialog() == DialogResult.OK)
            {
                this.panotoolsdir.Text = zgjidhDirektori.SelectedPath;
                this.panotoolsdirGlobal = zgjidhDirektori.SelectedPath.Replace("\\", "\\\\");
            }
        }

        private void watermarkBtn_Click(object sender, EventArgs e)
        {
            watermarkDialog.Filter = "*.png|*.png";
            watermarkDialog.ShowDialog();
            watermark.Text = watermarkDialog.FileName;
            this.watermarkFilename = watermarkDialog.FileName.Replace("\\", "\\\\");
        }

        private void imageMagickBtn_Click(object sender, EventArgs e)
        {
            if (zgjidhDirektori.ShowDialog() == DialogResult.OK)
            {
                this.imageMagickDir.Text = zgjidhDirektori.SelectedPath;
                this.ImageMagickPath = zgjidhDirektori.SelectedPath;
            }
        }

        private void trackBtn_Click(object sender, EventArgs e)
        {
            trackDialog.Filter = "*.txt|*.txt";
            trackDialog.ShowDialog();
            track.Text = trackDialog.FileName;
            this.trackFilename = trackDialog.FileName.Replace("\\", "\\\\");

        }
        /*
        private void trackCalc(int idPano)
        {
            Console.WriteLine("TrackCalc: " + idPano.ToString());
            // PhotoID, Data, Ora, Lat, Lng, Alt, Crs, Speed, Sats, Yaw, Pitch, Roll, Hdg
            StreamReader reader = File.OpenText(this.trackFilename);
            string writer = @initPathFolder + "\\" + Path.GetFileNameWithoutExtension(this.trackFilename) + ".csv";
            string line;
            int id;
            int lastId;
            if (albumi.Properties.Settings.Default.lastId > 0) id = albumi.Properties.Settings.Default.lastId;
            else id = 0;
            String rreshti2 = "";
            String oldOldId = "";

            while ((line = reader.ReadLine()) != null)
            {

                String rreshti = line.Replace('_', ',');
                String oldId = rreshti.Substring(0, rreshti.IndexOf(','));
                if (Int32.Parse(oldId).Equals(idPano))
                {
                    if (!oldId.Equals(oldOldId))
                    {

                        id++;
                        rreshti2 = rreshti2 + id + rreshti.Substring(rreshti.IndexOf(',')).ToString() + "," + oldId + "\r\n";
                        oldOldId = oldId;
                        //Console.WriteLine("TrackCalc,oldId: " + oldId.ToString() + " newId: " + id);
                        if (Directory.Exists(@panos + "\\3D_L" + oldId)) Directory.Move(@panos + "\\3D_L" + oldId, @final + "\\" + id);
                        else if (Directory.Exists(@panos + "\\3D_L0" + oldId)) Directory.Move(@panos + "\\3D_L0" + oldId, @final + "\\" + id);
                        else if (Directory.Exists(@panos + "\\3D_L00" + oldId)) Directory.Move(@panos + "\\3D_L00" + oldId, @final + "\\" + id);
                        else if (Directory.Exists(@panos + "\\3D_L000" + oldId)) Directory.Move(@panos + "\\3D_L000" + oldId, @final + "\\" + id);


                        //string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, "Renaming " + panos + "\\3D_L" + oldId + ".tif to " + @panos + "\\3D_L" + id + ".tif");
                        //this.sw.WriteLine(logLine);

                    }
                }
            }
            //if (File.Exists(writer)) File.Delete(writer);
            File.AppendAllText(writer, rreshti2);
            lastId = id;
            albumi.Properties.Settings.Default.lastId = lastId;
            initID.Text = lastId.ToString(); 
            //albumi.Properties.Settings.Default.lastId = 0;
            albumi.Properties.Settings.Default.Save();

            string[] tifs = Directory.GetFiles(@final + "\\" + id, "*.tif").ToArray();
            foreach (string f in tifs)
            {
                if (File.Exists(f)) File.Delete(f);
            }
        }
        */
        private void initID_TextChanged(object sender, EventArgs e)
        {
            if (initID.Text != "") albumi.Properties.Settings.Default.lastId = Int32.Parse(initID.Text);
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

       public void gotobottom()
        {
            this.status.SelectionStart = this.status.Text.Length;
            status.ScrollToCaret();

        }
          

        private void ndalBtn_Click(object sender, EventArgs e)
        {
            ndalo = true;
            MessageBox.Show("Programi do ndalohet sapo te mbaroje ky cikel");
        }

    }
}
