using PointLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Point = PointLib.Point;

namespace FormsApp
{
    public partial class PointForm : Form
    {
        private Point[] points = null;
        public PointForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            points = new Point[5];

            var rnd = new Random();

            for (int i = 0; i < points.Length; i++)
                points[i] = rnd.Next(3) % 2 == 0 ? new Point() : new Point3D();

            listBox.DataSource = points;


        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (points == null)
                return;

            Array.Sort(points);

            listBox.DataSource = null;
            listBox.DataSource = points;

        }

        private void btnSerialize_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            using (var fs =
                new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
            {
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".bin":
                        var bf = new BinaryFormatter();
                        bf.Serialize(fs, points);
                        break;
                    case ".soap":
                        var sf = new SoapFormatter();
                        sf.Serialize(fs, points);
                        break;
                    case ".xml":
                        var xf = new XmlSerializer(typeof(Point[]), new[] {typeof(Point3D)});
                        xf.Serialize(fs, points);
                        break;
                    case ".json":
                          
                     
                        using (StreamWriter sw = new StreamWriter(fs))
                       
                        {
                             
                            var json = JsonConvert.SerializeObject(points);
                            sw.Write(json);
                            //File.WriteAllText(fs, json);
                          
                        }
                        break;


                }

                }
            }


        

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            using (var fs =
                new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
            {
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".bin":
                        var bf = new BinaryFormatter();
                        points = (Point[])bf.Deserialize(fs);
                        break;
                    case ".soap":
                        var sf = new SoapFormatter();
                        points = (Point[])sf.Deserialize(fs);
                        break;
                    case ".xml":
                        var xf = new XmlSerializer(typeof(Point[]), new[] {typeof(Point3D)});
                        points = (Point[])xf.Deserialize(fs);
                        break;
                    case ".json":
                        using (StreamReader sw = new StreamReader(fs))
                        
                        {
                     
                            
                           // var json = JsonConvert.SerializeObject(points);
                          
                          //  MessageBox.Show(json);
                          var json = sw.ReadLine();
                          //MessageBox.Show(json);
                            var deserialized = JArray.Parse(json);
                            int i = 0;
                         
                             foreach (var element in deserialized)
                             {


                                 
                                 if (element["Z"] == null)
                                 {
                                     var p = new Point((int)element["X"], (int)element["Y"]);

                                     points[i] = p;
                                     
                                     // MessageBox.Show("2D");
                                 }
                                 else
                                 {
                                     var p = new Point3D((int)element["X"], (int)element["Y"],(int)element["Z"]);

                                     points[i] = p;
                                     //MessageBox.Show("3D");
                                 }
                                 //  MessageBox.Show(element["X"].ToString());
                                 i++;


                             }
                             
                        }

                        break;
         

                }
            }

            listBox.DataSource = null;
            listBox.DataSource = points;


        }

    }
}