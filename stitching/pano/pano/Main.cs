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
        String cam2;
        String cam3;
        String cam4;
        String cam5;
        String tmp;
        String tifs;
        // String tmp2;
        String panos;
        bool ndalo = false;
        String pto;
        String pto_mk;
        String logtxt;
        int files = 0;
        int remaining = 0;
        Bitmap img;
        //StreamWriter sw;
        String panotoolsdirGlobal;
        String panotoolsdirMobileGlobal;
        String watermarkFilename;
        String HuginPath;
        String ImageMagickPath;
        String trackFilename;
        String final;
        int threadcount = 0;
        Stopwatch sw = Stopwatch.StartNew();
        int nrThreads = 1;

        public Main()
        {
            InitializeComponent();


            if (albumi.Properties.Settings.Default.initDir.Length > 0)
            {
                this.initPathLabel.Text = albumi.Properties.Settings.Default.initDir;
                this.initPathFolder = this.initPathLabel.Text;
                this.cam1 = this.initPathFolder + "\\files\\1";
                this.cam2 = this.initPathFolder + "\\files\\2";
                this.cam3 = this.initPathFolder + "\\files\\3";
                this.cam4 = this.initPathFolder + "\\files\\4";
                this.cam5 = this.initPathFolder + "\\files\\5";
                instruksione.Text = "Instruksione:  " + this.initPathFolder + "\\files\\1...5";
                this.tmp = this.initPathFolder + "\\files\\tmp";
                //this.tmp2 = this.initPathFolder + "\\files\\tmp2";
                this.panos = this.initPathFolder + "\\files\\panos";
                this.tifs = this.initPathFolder + "\\files\\tifs";
                //this.logtxt = this.initPathFolder + "\\files\\log.txt";
                this.final = this.initPathFolder + "\\files\\final";

                if (mode1.Checked)
                {
                    TotFoto(this.cam1);
                    totFotos.Text = (files / 5).ToString();
                    remaining = files;
                    fotombetura.Text = (remaining / 5).ToString();
                    fotoperfunduara.Text = ((files - remaining) / 5).ToString();
                }
                else if(mode2.Checked)
                {
                    TotFoto(this.panos);
                    totFotos.Text = files.ToString();
                    remaining = files;
                    fotombetura.Text = remaining.ToString();
                    fotoperfunduara.Text = (files - remaining).ToString();
                } 
                fotoperfunduara.Refresh();

                if (!Directory.Exists(this.tmp))
                {
                    Directory.CreateDirectory(this.tmp);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                else
                {
                    Directory.Delete(this.tmp, true);
                    Directory.CreateDirectory(this.tmp);
                }
                if (!Directory.Exists(this.panos))
                {
                    Directory.CreateDirectory(this.panos);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                /*if (!Directory.Exists(this.tmp2))
                {
                    //Directory.CreateDirectory(this.tmp2);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }*/
                if (!Directory.Exists(this.final))
                {
                    Directory.CreateDirectory(this.final);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                if (!File.Exists(this.logtxt))
                {
                    //this.sw = new StreamWriter(this.logtxt);
                }
                else
                {
                    // File.Delete(this.logtxt);
                    // this.sw = new StreamWriter(this.logtxt);
                }
            }
            if (albumi.Properties.Settings.Default.initPto.Length > 0)
            {
                this.initPtoLabel.Text = albumi.Properties.Settings.Default.initPto;
                this.pto = this.initPtoLabel.Text.Replace("\\", "\\\\");
            }
            if (albumi.Properties.Settings.Default.huginPath.Length > 0)
            {
                this.huginDir.Text = albumi.Properties.Settings.Default.huginPath;
                this.HuginPath = this.huginDir.Text.Replace("\\", "\\\\");
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
            if (albumi.Properties.Settings.Default.track.Length > 0)
            {
                this.track.Text = albumi.Properties.Settings.Default.track;
                this.trackFilename = this.track.Text.Replace("\\", "\\\\");
            }
            if (albumi.Properties.Settings.Default.track.Length > 0)
            {
                this.watermark.Text = albumi.Properties.Settings.Default.watermark;
                this.watermarkFilename = this.watermark.Text.Replace("\\", "\\\\");
            }
            this.pitchcheck.Checked = albumi.Properties.Settings.Default.pitchcheck;
           
            
            initID.Text = albumi.Properties.Settings.Default.lastId.ToString();

        }

        private void folderPath_Click(object sender, EventArgs e)
        {
            if (zgjidhDirektori.ShowDialog() == DialogResult.OK)
            {
                this.initPathLabel.Text = zgjidhDirektori.SelectedPath;
                this.initPathFolder = zgjidhDirektori.SelectedPath;
                this.initPathLabel.Text = this.initPathFolder;
                this.cam1 = this.initPathFolder + "\\files\\1";
                this.cam2 = this.initPathFolder + "\\files\\2";
                this.cam3 = this.initPathFolder + "\\files\\3";
                this.cam4 = this.initPathFolder + "\\files\\4";
                this.cam5 = this.initPathFolder + "\\files\\5";
                instruksione.Text = "Instruksione:  " +this.initPathFolder + "\\files\\1...5";
                this.tmp = this.initPathFolder + "\\files\\tmp";
                //this.tmp2 = this.initPathFolder + "\\files\\tmp2";
                this.panos = this.initPathFolder + "\\files\\panos";
                this.tifs = this.initPathFolder + "\\files\\tifs";
                //this.logtxt = this.initPathFolder + "\\files\\log.txt";
                this.final = this.initPathFolder + "\\files\\final";
                /*
                initPtoLabel.Text = this.initPathFolder + "\\apps\\pano.pto";
                huginDir.Text = this.initPathFolder + "\\apps\\Hugin";
                imageMagickDir.Text = this.initPathFolder + "\\apps\\ImageMagick";
                panotoolsdir.Text = this.initPathFolder + "\\apps\\krpanotools";
                watermark.Text = this.initPathFolder + "\\apps\\watermark.png";
                track.Text = this.initPathFolder + "\\files\\TRACK.txt";
                this.trackFilename = this.track.Text.Replace("\\", "\\\\");
                */
                if (mode1.Checked)
                {
                    TotFoto(this.cam1);
                    totFotos.Text = (files / 5).ToString();
                    remaining = files;
                    fotombetura.Text = (remaining / 5).ToString();
                    fotoperfunduara.Text = ((files - remaining) / 5).ToString();
                }
                else if (mode2.Checked)
                {
                    TotFoto(this.panos);
                    totFotos.Text = files.ToString();
                    remaining = files;
                    fotombetura.Text = remaining.ToString();
                    fotoperfunduara.Text = (files - remaining).ToString();
                }
                fotoperfunduara.Refresh();

                if (!File.Exists(this.logtxt))
                {
                    //this.sw = new StreamWriter(this.logtxt);
                }
                else
                {
                    //File.Delete(this.logtxt);
                    //this.sw = new StreamWriter(this.logtxt);
                }

                if (!Directory.Exists(this.tmp))
                {
                    Directory.CreateDirectory(this.tmp);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                if (!Directory.Exists(this.panos))
                {
                    Directory.CreateDirectory(this.panos);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                if (!Directory.Exists(this.final))
                {
                    Directory.CreateDirectory(this.final);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                if (!Directory.Exists(this.tifs))
                {
                    Directory.CreateDirectory(this.tifs);
                    //Console.WriteLine("Create tmp: "+this.tmp);
                }
                /* if (!Directory.Exists(this.tmp2))
                 {
                     //Directory.CreateDirectory(this.tmp2);
                     //Console.WriteLine("Create tmp: "+this.tmp);
                 }*/

            }
        }

        private void initPtoBtn_Click(object sender, EventArgs e)
        {
            initPto.Filter = "*.pto|*.pto";
            initPto.ShowDialog();
            initPtoLabel.Text = initPto.FileName;
            this.pto = initPto.FileName;
            this.pto_mk = initPto.FileName + ".mk";
        }


        public void TotFoto(string dir)
        {
            dir = dir + @"\";
            files = 0;
            if (Directory.Exists(dir))
            {
                String[] all_files = Directory.GetFileSystemEntries(dir);

                foreach (string file in all_files)
                {
                    if (Directory.Exists(file))
                    {
                        TotFoto(file);
                    }
                    else
                    {
                        if (mode2.Checked && Path.GetExtension(file).Equals(".tif")) { files++; } 
                        else if(mode1.Checked) files++;
                    }
                }
                if(mode1.Checked) files = files * 5;
            }
        }

        
        public void SingleMode(string foto)
        {
           
                Console.WriteLine("SingleMode: " + foto.ToString());

                if (File.Exists(foto))
                {
                                 Directory.CreateDirectory(this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto));

                                 
                                 fotoperfunduara.Refresh();

                                 
                                if (File.Exists(foto))
                                {
                                    Directory.CreateDirectory(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));
                                    File.Move(foto, this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileName(foto));
                                    if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif")) File.Copy(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif", this.tifs + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");  
                                }
                                 
                                 status.Text = status.Text + "\r\nImageMagick po pergatit foton e vogel " + Path.GetFileNameWithoutExtension(foto);
                                 executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif -resize 240 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg");
                                 executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg  -crop 240x120-60-20 -crop 404x202+60+20 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg");
                               
                                 status.Text = status.Text + "\r\nKrpano po e ben copa copa foton " + Path.GetFileNameWithoutExtension(foto);
                                 gotobottom();
                                 executeExe(this.panotoolsdirGlobal + "\\krpanotools64.exe", " makepano -config=" + this.panotoolsdirGlobal + "\\templates\\multires.config " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");

                                 if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif")) File.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");

                                 status.Text = status.Text + "\r\nImageMagick po e mbron foton " + Path.GetFileNameWithoutExtension(foto) + " nga hajdutet";
                                 gotobottom();
                                 WatermarkPhotos(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));                                
                                
                                 status.Text = status.Text + "\r\nTrack po ben llogaritje astronomike: " + Path.GetFileNameWithoutExtension(foto);
                                 trackCalc(Int32.Parse(Path.GetFileNameWithoutExtension(foto)));

                                 status.Text = status.Text + "\r\nProgrami po ben pastrimin e shtepise: " + Path.GetFileNameWithoutExtension(foto);
                                 //moveToFinal(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));

                                 status.Text = status.Text + "\r\nTrete koshin!!!!";
                                 SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOSOUND | RecycleFlags.SHERB_NOCONFIRMATION);
                                 gotobottom();
                                 status.Text = status.Text + "\r\n"+ DateTime.Now;

                                 TotFoto(this.panos);
                                 totFotos.Text = files.ToString();
                                 remaining = files;
                                 fotombetura.Text = remaining.ToString();
                                 fotoperfunduara.Text = (files - remaining).ToString();
                                 fotoperfunduara.Refresh();
                                 this.Refresh();
                                 this.Update();
                                 this.threadcount--;        
                }
           
        }


        public void MovePhotosThread(string foto)
        {

            Console.WriteLine("MovePhotosThread: " + foto.ToString());

            if (File.Exists(foto))
            {
                //Console.WriteLine("MovePhotosThread img: " + foto.ToString());
                Directory.CreateDirectory(this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto));

                if (File.Exists(this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto")) File.Delete(this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto");
                System.IO.File.Copy(this.initPtoLabel.Text, this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto");
                this.pto = this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto";

                /*{
                    System.IO.File.Copy(this.initPtoLabel.Text, this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto");
                    this.pto = this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pano.pto";
                }*/


                if (File.Exists(this.cam1 + "\\" + Path.GetFileName(foto))) File.Move(this.cam1 + "\\" + Path.GetFileName(foto), this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\1.jpg");
                if (File.Exists(this.cam2 + "\\" + Path.GetFileName(foto))) File.Move(this.cam2 + "\\" + Path.GetFileName(foto), this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\2.jpg");
                if (File.Exists(this.cam3 + "\\" + Path.GetFileName(foto))) File.Move(this.cam3 + "\\" + Path.GetFileName(foto), this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\3.jpg");
                if (File.Exists(this.cam4 + "\\" + Path.GetFileName(foto))) File.Move(this.cam4 + "\\" + Path.GetFileName(foto), this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\4.jpg");
                if (File.Exists(this.cam5 + "\\" + Path.GetFileName(foto))) File.Move(this.cam5 + "\\" + Path.GetFileName(foto), this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\5.jpg");

                executeExe(this.ImageMagickPath + "\\convert", " " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\1.jpg -rotate 90 -compress none " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\1.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\2.jpg -rotate 90 -compress none " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\2.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\3.jpg -rotate 90 -compress none " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\3.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\4.jpg -rotate 90 -compress none " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\4.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\5.jpg -rotate 90 -compress none " + this.tmp + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\5.tif");

                if (pitchcheck.Checked)
                {
                    status.Text = status.Text + "\r\nTransform-pano po drejton foton " + Path.GetFileNameWithoutExtension(foto);
                    gotobottom();
                    String rememberID = Path.GetFileNameWithoutExtension(foto);
                    StreamReader reader = File.OpenText(this.trackFilename);
                    string line;
                    String pitch = "";
                    String roll = "";
                    int yaw = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        String oldId = line.Substring(0, line.IndexOf('_'));
                        if (Int32.Parse(rememberID).Equals(Int32.Parse(oldId)))
                        {
                            int pitchPos = line.IndexOf("pitch");
                            int rollPos = line.IndexOf("roll");
                            int hdgPos = line.Length;

                            Console.WriteLine(line);
                            pitch = line.Substring(pitchPos, (rollPos - pitchPos)).Replace("pitch", "").Replace("_", "");
                            roll = line.Substring(rollPos, (hdgPos - rollPos)).Replace("roll", "").Replace("_", "");
                            Console.WriteLine(pitch + " " + roll);
                        }
                    }

                    executeExe(this.HuginPath + "\\bin\\transform-pano", " " + roll + " " + pitch + " " + yaw + " " + this.pto + " " + this.pto);
                }

                status.Text = status.Text + "\r\nvig_optimiser po zbukuron foton " + Path.GetFileNameWithoutExtension(foto);
                gotobottom();
                executeExe(this.HuginPath + "\\bin\\vig_optimize", " -p 200 -o " + this.pto + " " + this.pto);


                status.Text = status.Text + "\r\nHugin po ben spektakel per foton " + Path.GetFileNameWithoutExtension(foto);
                gotobottom();
                Directory.CreateDirectory(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));
                executeExe(this.HuginPath + "\\bin\\hugin_stitch_project", " --output=" + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + " " + this.pto);

                remaining = remaining - 5;

                fotombetura.Text = (remaining / 5).ToString();
                fotombetura.Refresh();
                fotoperfunduara.Text = ((this.files - remaining) / 5).ToString();
                fotoperfunduara.Refresh();

                string[] tifs = Directory.GetFiles(this.panos, "*.tif").ToArray();

                foreach (string f in tifs)
                {
                    if (!Path.GetFileName(f).Equals(Path.GetFileNameWithoutExtension(foto) + "_fused.tif"))
                    {
                        File.Delete(f);
                    }
                }
                tifs = Directory.GetFiles(this.panos, "*.jpg").ToArray();

                foreach (string f in tifs)
                {
                        File.Delete(f);
                }

                status.Text = status.Text + "\r\nImageMagick po zbukuron foton " + Path.GetFileNameWithoutExtension(foto);
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "_fused.tif -normalize " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");
                if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "_fused.tif")) File.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "_fused.tif");

                //executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif -background #808080 -flatten " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");
                
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif -crop 6432x10+0+500 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif -resize \"6432x510!\" -blur 0x50 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif");
                //C:\\albumi\\apps\\mask2.png 
                executeExe(this.ImageMagickPath + "\\composite", " -gravity north " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");


                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif -crop 6432x10+0+2640 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif -resize \"6432x576!\" -blur 0x50 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif");
                //C:\\albumi\\apps\\mask1.png
                executeExe(this.ImageMagickPath + "\\composite", " -gravity south " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");
               
                if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif")) File.Copy(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif", this.tifs + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");

                status.Text = status.Text + "\r\nImageMagick po pergatit foton e vogel " + Path.GetFileNameWithoutExtension(foto);
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif -resize 240 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg");
                executeExe(this.ImageMagickPath + "\\convert", " " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg  -crop 240x120-60-20 -crop 404x202+60+20 " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\thumb_big.jpg");

                if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif")) File.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa2.tif");
                if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif")) File.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\pjesa3.tif");
               
                status.Text = status.Text + "\r\nKrpano po e ben copa copa foton " + Path.GetFileNameWithoutExtension(foto);
                gotobottom();
                executeExe(this.panotoolsdirGlobal + "\\krpanotools64.exe", " makepano -config=" + this.panotoolsdirGlobal + "\\templates\\multires.config " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");
                Console.WriteLine(this.panotoolsdirGlobal + "\\krpanotools64.exe " + "makepano -config=" + this.panotoolsdirGlobal + "\\templates\\multires.config " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");

                status.Text = status.Text + "\r\nBajme punet e shpise " + Path.GetFileNameWithoutExtension(foto);
                //if (!Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto))) Directory.CreateDirectory(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto));
                //if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif")) File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif");

                // if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif")) File.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");

                // executeExe(this.panotoolsmobiledir.Text + "\\kmakemultires", " -config=" + this.panotoolsmobiledir.Text + "\\templates\\multires.config " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");
                //Console.WriteLine(this.panotoolsmobiledir.Text + "\\kmakemultires" + " -config=" + this.panotoolsmobiledir.Text + "\\templates\\multires.config " + this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + ".tif");


                status.Text = status.Text + "\r\nImageMagick po e mbron foton " + Path.GetFileNameWithoutExtension(foto) + " nga hajdutet";
                gotobottom();
                WatermarkPhotos(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));

                //if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif")) File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif");

                //if (File.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif"))
                //{
                //    File.Move(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\preview.tif", this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\mobile\\preview.tif");
                //}

                //if (Directory.Exists(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto))) Directory.Delete(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto) + "\\" + Path.GetFileNameWithoutExtension(foto));
        
                status.Text = status.Text + "\r\nTrack po ben llogaritje astronomike: " + Path.GetFileNameWithoutExtension(foto);
                trackCalc(Int32.Parse(Path.GetFileNameWithoutExtension(foto)));

                status.Text = status.Text + "\r\nProgrami po ben pastrimin e shtepise: " + Path.GetFileNameWithoutExtension(foto);
                //moveToFinal(this.panos + "\\" + Path.GetFileNameWithoutExtension(foto));

                status.Text = status.Text + "\r\nTrete koshin!!!!";
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOSOUND | RecycleFlags.SHERB_NOCONFIRMATION);
                gotobottom();
                status.Text = status.Text + "\r\n" + DateTime.Now;

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
                    if (Path.GetExtension(file) == ".tif" && Path.GetFileNameWithoutExtension(file) != "preview_mobile" && Path.GetFileNameWithoutExtension(file) != "thumb_big" && Path.GetFileNameWithoutExtension(file) != "thumb" && Path.GetFileNameWithoutExtension(file) != "preview" && Path.GetFileNameWithoutExtension(file) != Path.GetDirectoryName(dir))
                    {
                        executeExe(this.ImageMagickPath + "\\composite", " -dissolve 100% -gravity Center " + this.watermarkFilename + " " + file + " " + file);
                    }
                }

                executeExe(this.ImageMagickPath + "\\mogrify", " -format jpg -quality 80 " + dir + "\\*.tif");

                foreach (string d in Directory.GetDirectories(dir))
                {
                    WatermarkPhotos(d);
                }                
            }
        }

      

        public void executeExe(string filename, string args)
        {
            Console.WriteLine("executeExe: " + filename+" "+args);
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

        private void eraseTmp()
        {
            if (Directory.Exists(this.tmp))
            {
                string[] deletetmp = Directory.GetDirectories(this.tmp, "*");
                foreach (string f in deletetmp)
                {
                    deleteFile(f);
                }
            }
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

        private void moveToFinal(string directory)
        {
            /*string[] tifs = Directory.GetFiles(this.panos, "*.tif").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
           
            foreach (string f in tifs)
            {
                File.Delete(f);
            }
            
            if (!Directory.Exists(this.final + "\\" + directory.ToString().Substring((directory.LastIndexOf("\\")), (directory.Length - directory.LastIndexOf("\\"))).Replace("\\", ""))) Directory.CreateDirectory(this.final + "\\" + directory.ToString().Substring((directory.LastIndexOf("\\")), (directory.Length - directory.LastIndexOf("\\"))).Replace("\\", ""));
            string dest = this.final + "\\" + directory.ToString().Substring(directory.LastIndexOf("\\"), (directory.Length - directory.LastIndexOf("\\"))).Replace("\\", "");
            String dir = directory + @"\";
            
            string[] tifs = Directory.GetFiles(dir, "*.tif").ToArray();
            foreach (string f in tifs)
            {
                File.Delete(f);
            }

            string[] jpgs = Directory.GetFiles(dir, "*.jpg").ToArray();
            foreach (string f in jpgs)
            {
                if (File.Exists(f)) File.Move(f, dest + "\\" + Path.GetFileName(f));
            }

            System.IO.Directory.Delete(dir);  
             */
        }


        private void start_Click(object sender, EventArgs e)
        {
            ndalo = false;
            folderPath.Enabled = false;
            initPathLabel.Enabled = false;
            initPtoBtn.Enabled = false;
            initPtoLabel.Enabled = false;
            huginBtn.Enabled = false;
            huginDir.Enabled = false;
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
            pitchcheck.Enabled = false;

            albumi.Properties.Settings.Default.initDir = this.initPathLabel.Text;
            albumi.Properties.Settings.Default.initPto = this.initPtoLabel.Text;
            albumi.Properties.Settings.Default.huginPath = this.huginDir.Text;
            albumi.Properties.Settings.Default.imageMagicPath = this.imageMagickDir.Text;
            albumi.Properties.Settings.Default.krpanotoolsPath = this.panotoolsdir.Text;
            albumi.Properties.Settings.Default.watermark = this.watermark.Text;
            albumi.Properties.Settings.Default.track = this.track.Text;
            albumi.Properties.Settings.Default.pitchcheck = this.pitchcheck.Checked;
            albumi.Properties.Settings.Default.Save();

             if (mode1.Checked)
            {
            if (!File.Exists(this.track.Text) || !File.Exists(this.watermark.Text) || !Directory.Exists(this.panotoolsdir.Text) || !Directory.Exists(this.imageMagickDir.Text) || !Directory.Exists(this.huginDir.Text)
                || !File.Exists(this.initPtoLabel.Text) || !Directory.Exists(this.initPathLabel.Text))
            {
                MessageBox.Show("Nje ose me shume direktori ose file mungojne!");
                folderPath.Enabled = true;
                initPathLabel.Enabled = true;
                initPtoBtn.Enabled = true;
                initPtoLabel.Enabled = true;
                huginBtn.Enabled = true;
                huginDir.Enabled = true;
                imageMagickBtn.Enabled = true;
                imageMagickDir.Enabled = true;
                panotoolsBtn.Enabled = true;
                panotoolsdir.Enabled = true;
                watermark.Enabled = true;
                watermarkBtn.Enabled = true;
                track.Enabled = true;
                trackBtn.Enabled = true;
                initID.Enabled = true;
                pitchcheck.Enabled = true;
                return;
            }
            if (!File.Exists(this.huginDir.Text + "\\bin\\transform-pano.exe"))
            {
                MessageBox.Show("transform-pano tek direktoria e Hugin mungon!");
                folderPath.Enabled = true;
                initPathLabel.Enabled = true;
                initPtoBtn.Enabled = true;
                initPtoLabel.Enabled = true;
                huginBtn.Enabled = true;
                huginDir.Enabled = true;
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
                pitchcheck.Enabled = true;
                return;
            }
            }

            if (mode1.Checked)
            {
                TotFoto(this.cam1);
                totFotos.Text = (files / 5).ToString();
                remaining = files;
                fotombetura.Text = (remaining / 5).ToString();
                fotoperfunduara.Text = ((files - remaining) / 5).ToString();
            }
            else if (mode2.Checked)
            {
                TotFoto(this.panos);
                totFotos.Text = files.ToString();
                remaining = files;
                fotombetura.Text = remaining.ToString();
                fotoperfunduara.Text = (files - remaining).ToString();
            }
            fotoperfunduara.Refresh();

            if (mode1.Checked)
            {
            string[] cam11 = Directory.GetFiles(this.cam1);
            string[] cam22 = Directory.GetFiles(this.cam2);
            string[] cam33 = Directory.GetFiles(this.cam3);
            string[] cam44 = Directory.GetFiles(this.cam4);
            string[] cam55 = Directory.GetFiles(this.cam5);

            if (!((int)cam11.Length == (int)cam22.Length || (int)cam11.Length == (int)cam33.Length || (int)cam11.Length == (int)cam44.Length || (int)cam11.Length == (int)cam55.Length) ||
                !((int)cam22.Length == (int)cam11.Length || (int)cam22.Length == (int)cam33.Length || (int)cam22.Length == (int)cam44.Length || (int)cam22.Length == (int)cam55.Length) ||
                !((int)cam33.Length == (int)cam22.Length || (int)cam33.Length == (int)cam11.Length || (int)cam33.Length == (int)cam44.Length || (int)cam33.Length == (int)cam55.Length) ||
                !((int)cam44.Length == (int)cam22.Length || (int)cam44.Length == (int)cam33.Length || (int)cam44.Length == (int)cam11.Length || (int)cam44.Length == (int)cam55.Length) ||
                !((int)cam55.Length == (int)cam22.Length || (int)cam55.Length == (int)cam33.Length || (int)cam55.Length == (int)cam44.Length || (int)cam55.Length == (int)cam11.Length)
                )
            {
                MessageBox.Show("Numri i fotove ne direktorite 1 2 3 4 5 nuk eshte i njejte!");
                return;
            }


            if (Directory.Exists(this.panos))
            {
                string[] tmppics = Directory.GetFiles(this.panos, "*").ToArray();

                foreach (string f in tmppics)
                {
                    File.Delete(f);
                }
            }

            }

            eraseTmp();

            if (!Directory.Exists(this.tifs))
            {
                Directory.CreateDirectory(this.tifs);
                //Console.WriteLine("Create tmp: "+this.tmp);
            }

            if (File.Exists(this.tmp + "\\" + Path.GetFileName(this.pto))) File.Delete(this.tmp + "\\" + Path.GetFileName(this.pto));
            if (File.Exists(this.tmp + "\\" + Path.GetFileName(this.pto_mk))) File.Delete(this.tmp + "\\" + Path.GetFileName(this.pto_mk));
            //System.IO.File.Copy(this.tmp2 + "\\" + Path.GetFileName(this.pto), this.tmp + "\\" + Path.GetFileName(this.pto));
            //System.IO.File.Copy(this.tmp2 + "\\" + Path.GetFileName(this.pto_mk), this.tmp + "\\" + Path.GetFileName(this.pto_mk));
            this.pto = this.tmp + "\\" + Path.GetFileName(this.pto);
            this.pto_mk = this.tmp + "\\" + Path.GetFileName(this.pto_mk);
            string[] threads;

            if (mode2.Checked)
            {
                threads = Directory.GetFiles(this.panos, "*.tif").ToArray();
            }
            else if (mode1.Checked)
            {
                threads = Directory.GetFiles(this.cam1, "*.JPG").ToArray();
            }
            else
            {
                threads = Directory.GetFiles(this.cam1, "*.JPG").ToArray();
            }

             foreach (string f in threads)
             {
                 if (ndalo == false)
                 {
                     if (mode2.Checked)
                     {
                         SingleMode(f);
                     }
                     else if (mode1.Checked)
                     {
                         MovePhotosThread(f);
                     }
                 }
                 else
                 {
                     sw.Stop();
                     TimeSpan elapsedTime = sw.Elapsed;
                     string elapsedTimeFormated = String.Format("{0:00} dite {1:00} ore {2:00} min {3:00} sek", elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                     status.Text = status.Text + "\r\nKreu per: " + elapsedTimeFormated + " ne oren: " + DateTime.Now;
                     gotobottom();
                     folderPath.Enabled = true;
                     initPathLabel.Enabled = true;
                     initPtoBtn.Enabled = true;
                     initPtoLabel.Enabled = true;
                     huginBtn.Enabled = true;
                     huginDir.Enabled = true;
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
                     pitchcheck.Enabled = true;
                     break;
                 }
            }

             if (remaining == 0)
             {
                 sw.Stop();
                 TimeSpan elapsedTime = sw.Elapsed;
                 string elapsedTimeFormated = String.Format("{0:00} dite {1:00} ore {2:00} min {3:00} sek", elapsedTime.Days, elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                 status.Text = status.Text + "\r\nKreu per: " + elapsedTimeFormated + " ne oren: " + DateTime.Now;
                 gotobottom();
                 folderPath.Enabled = true;
                 initPathLabel.Enabled = true;
                 initPtoBtn.Enabled = true;
                 initPtoLabel.Enabled = true;
                 huginBtn.Enabled = true;
                 huginDir.Enabled = true;
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
                 pitchcheck.Enabled = true;
 
             }
            //);

            //MovePhotos(this.cam1);

            //executeExe(this.panotoolsdirGlobal + "\\kmakemultires", " " + this.panos + "\\*.tif");

            //WatermarkPhotos(this.panos);

            //moveToFinal();


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
                     initPtoBtn.Enabled = true;
                     initPtoLabel.Enabled = true;
                     huginBtn.Enabled = true;
                     huginDir.Enabled = true;
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
                     pitchcheck.Enabled = true;                
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

        private void huginBtn_Click(object sender, EventArgs e)
        {
            if (zgjidhDirektori.ShowDialog() == DialogResult.OK)
            {
                this.huginDir.Text = zgjidhDirektori.SelectedPath;
                this.HuginPath = zgjidhDirektori.SelectedPath.Replace("\\", "\\\\");
            }
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
        private void trackCalc(){
            // PhotoID, Data, Ora, Lat, Lng, Alt, Crs, Speed, Sats, Yaw, Pitch, Roll, Hdg
            StreamReader reader = File.OpenText(this.trackFilename);
            string writer = @initPathFolder + "\\files\\" + Path.GetFileNameWithoutExtension(this.trackFilename) + ".csv";  
            string line;
            int id;
            int lastId;
            if (albumi.Properties.Settings.Default.lastId > 0) id = albumi.Properties.Settings.Default.lastId;
            else id = 0;
            String rreshti2 = "";
            String oldOldId = "";

            while ((line = reader.ReadLine()) != null) {
                              
                String rreshti = line.Replace('_',',');
                String oldId = rreshti.Substring(0, rreshti.IndexOf(','));
                if(!oldId.Equals(oldOldId)){
                    id++;
                    rreshti2 = rreshti2 + id + rreshti.Substring(rreshti.IndexOf(',')).ToString()+","+oldId+"\r\n";
                    oldOldId = oldId;
                    if(File.Exists(@panos+"\\3D_L"+oldId+".tif"))File.Move(@panos+"\\3D_L"+oldId+".tif", @panos+"\\3D_L"+id+".tif");
                    //string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, "Renaming " + panos + "\\3D_L" + oldId + ".tif to " + @panos + "\\3D_L" + id + ".tif");
                    //this.sw.WriteLine(logLine);
                   
                }
            }
            if (File.Exists(writer)) File.Delete(writer);
            File.WriteAllText(writer, rreshti2);
            lastId = id;
            albumi.Properties.Settings.Default.lastId = lastId;
           // albumi.Properties.Settings.Default.lastId = 0;
            albumi.Properties.Settings.Default.Save();
        }

        */

        private void trackCalc(int idPano)
        {
            Console.WriteLine("TrackCalc: " + idPano.ToString());
            // PhotoID, Data, Ora, Lat, Lng, Alt, Crs, Speed, Sats, Yaw, Pitch, Roll, Hdg
            StreamReader reader = File.OpenText(this.trackFilename);
            string writer = @initPathFolder + "\\files\\" + Path.GetFileNameWithoutExtension(this.trackFilename) + ".csv";
            string line;
            int id;
            int lastId;
            if (albumi.Properties.Settings.Default.lastId > 0) id = albumi.Properties.Settings.Default.lastId;
            else id = 0;
            String rreshti2 = "";
            String oldOldId = "";
            String oldFilename = "";

            while ((line = reader.ReadLine()) != null)
            {

                String rreshti = line.Replace('_', ',');
                String oldId = rreshti.Substring(0, rreshti.IndexOf(','));
               // if (Int32.Parse(oldId).Equals(idPano))
                //{
                   // if (!oldId.Equals(oldOldId))
                    //{

                        id++;
                        rreshti2 = rreshti2 +  id + "," + rreshti.Substring(rreshti.IndexOf(',')+1).ToString() + "\r\n";
                        oldOldId = oldId;

                        if(mode1.Checked){
                        if (Directory.Exists(@panos + "\\" + oldId)) { Directory.Move(@panos + "\\" + oldId, @final + "\\" + id); oldFilename = "" + oldId; }
                        else if (Directory.Exists(@panos + "\\0" + oldId)) { Directory.Move(@panos + "\\0" + oldId, @final + "\\" + id); oldFilename = "0" + oldId; }
                        else if (Directory.Exists(@panos + "\\00" + oldId)) { Directory.Move(@panos + "\\00" + oldId, @final + "\\" + id); oldFilename = "00" + oldId; }
                        else if (Directory.Exists(@panos + "\\000" + oldId)) { Directory.Move(@panos + "\\000" + oldId, @final + "\\" + id); oldFilename = "000" + oldId; }
                        }
                        else if (mode2.Checked) {
                            if (Directory.Exists(@panos + "\\" + oldId)) { Directory.Move(@panos + "\\" + oldId, @final + "\\" + id); oldFilename = oldId; }
                        }
                        if (File.Exists(this.tifs + "\\" + oldFilename + ".tif")) { File.Move(this.tifs + "\\" + oldFilename + ".tif", this.tifs + "\\" + id + ".tif"); }
                        //string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now, "Renaming " + panos + "\\3D_L" + oldId + ".tif to " + @panos + "\\3D_L" + id + ".tif");
                        //this.sw.WriteLine(logLine);

                //    }
               // }
                    reader.Close();
                var lines = System.IO.File.ReadAllLines(this.trackFilename);
                System.IO.File.WriteAllLines(this.trackFilename, lines.Skip(1).ToArray());
                break;
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
            if (File.Exists(this.final + "\\" + id + "\\" + oldFilename + ".jpg")) File.Delete(this.final + "\\" + id + "\\" + oldFilename + ".jpg");

        }

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
     
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ndalBtn_Click(object sender, EventArgs e)
        {
            ndalo = true;
            MessageBox.Show("Programi do ndalohet sapo te mbaroje ky cikel");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void mode2_CheckedChanged(object sender, EventArgs e)
        {
            instruksione.Text = "Instruksione:  " + this.initPathFolder + "\\files\\panos\\ID.tif \n\r" +
               " (1.tif, 2.tif etj sipas TRACK.txt). Dimensione: 3216x1608";
            TotFoto(this.panos);
            totFotos.Text = files.ToString();
            remaining = files;
            fotombetura.Text = remaining.ToString();
            fotoperfunduara.Text = (files - remaining).ToString();
            fotoperfunduara.Refresh();
        }

        private void mode1_CheckedChanged(object sender, EventArgs e)
        {
            instruksione.Text = "Instruksione:  " + this.initPathFolder + "\\files\\1...5";
            TotFoto(this.cam1);
            totFotos.Text = (files / 5).ToString();
            remaining = files;
            fotombetura.Text = (remaining / 5).ToString();
            fotoperfunduara.Text = ((files - remaining) / 5).ToString();
            fotoperfunduara.Refresh();
        }

    }
}
