using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ProyectoEtiquetaAviso
{
    public enum eMarca
    {
        Nada,
        Cruz,
        Circulo,
        ImagenDeForma
    }
    [
    DefaultProperty("Gradiente"),
    DefaultEvent("Load")
 ]
    public partial class EtiquetaAviso : Control
    {
        
        public EtiquetaAviso()
        {
            InitializeComponent();
        }

        public int tamaño;

        private Bitmap imagenMarca;
        [Category("Appareance")]
        [Description("Imagen que se pondra en marca")]
        public Bitmap ImagenMarca
        {
            set
            {
                imagenMarca = value;
                this.Refresh();
            }
            get
            {
                return imagenMarca;
            }
        }

        private Color colorInicioGradiente;
        [Category("Appareance")]
        [Description("Color inicial del gradiente")]
        public Color ColorInicioGradiente
        {
            set
            {
                colorInicioGradiente = value;
                this.Refresh();
            }
            get
            {
                return colorInicioGradiente;
            }
        }

        private Color colorFinalGradiente;
        [Category("Appareance")]
        [Description("Color final del gradiente")]
        public Color ColorFinalGradiente
        {
            set
            {
                colorFinalGradiente = value;
                this.Refresh();              
            }
            get
            {
                return colorFinalGradiente;
            }
        }

        private bool gradiente = false;
        [Category("Appareance")]
        [Description("Pone de fondo un gradiente")]
        public bool Gradiente
        {
            set
            {
                gradiente = value;
                this.Refresh(); //necesario para que se actualice el valor
            }
            get
            {
                return gradiente;
            }
        }
        private eMarca marca = eMarca.Nada;

        [Category("Appearance")]
        [Description("Apareca antes del texto una cruz, un círculo o nada")]
        public eMarca Marca
        {
            set
            {
                marca = value;
                this.Refresh();
            }
            get
            {
                return marca;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            dibujarMarcaYTexto(e);

            if (gradiente)
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(
                new Point(0, 10),
                new Point(10, 20),
                ColorInicioGradiente,
                colorFinalGradiente);

                Pen pen = new Pen(gradientBrush, this.Width);

                //e.Graphics.DrawLine(pen, 0, 0, this.Location.X, 0);
                e.Graphics.DrawLine(pen, 0, 0, this.Width, this.Height);
                dibujarMarcaYTexto(e);
            }
        }

        private void dibujarMarcaYTexto(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int grosor = 0; //Grosor de las líneas de dibujo
            int offsetX = 0; //Desplazamiento a la derecha del texto
            int offsetY = 0; //Desplazamiento hacia abajo del texto

            //Esta propiedad provoca mejoras en la apariencia o en la eficiencia
            // a la hora de dibujar
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Dependiendo del valor de la propiedad marca dibujamos una cruz o un Círculo
            switch (Marca)
            {
                case eMarca.Circulo:
                    grosor = 20;
                    g.DrawEllipse(new Pen(Color.Green, grosor), grosor, grosor,
                   this.Font.Height, this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor;
                    break;

                case eMarca.Cruz:
                    grosor = 5;
                    Pen lapiz = new Pen(Color.Red, grosor);
                    g.DrawLine(lapiz, grosor, grosor, this.Font.Height,
                   this.Font.Height);
                    g.DrawLine(lapiz, this.Font.Height, grosor, grosor,
                   this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor / 2;
                    //Es recomendable liberar recursos de dibujo pues se
                    //pueden realizar muchos y cogen memoria
                    lapiz.Dispose();
                    break;

                case eMarca.ImagenDeForma:

                    if (ImagenMarca == null)
                    {
                        break;
                    }
                    else
                    {
                        grosor = 5;
                        g.DrawImage(ImagenMarca, grosor, grosor, this.Font.Height, this.Font.Height);
                        offsetX = grosor + this.Font.Height;
                        offsetY = grosor;
                    }

                    //grosor = 20;
                    //g.DrawImage(ImagenMarca, 0, 0, 100, 100);
                    //offsetX = this.Font.Height+grosor;
                    //offsetY = grosor;       
                    break;
            }
            //Finalmente pintamos el Texto; desplazado si fuera necesario
            SolidBrush b = new SolidBrush(Color.Black);
            g.DrawString(this.Text, this.Font, b, offsetX + grosor, offsetY);
            Size tam = g.MeasureString(this.Text, this.Font).ToSize();
            this.Size = new Size(tam.Width + offsetX + grosor, tam.Height + offsetY * 2);
            b.Dispose();
            tamaño = offsetX + offsetY;
        }
        public event System.EventHandler ClickEnMarca;

        private void EtiquetaAviso_MouseClick(object sender, MouseEventArgs e) {
            if (e.X<tamaño)
            {
                ClickEnMarca?.Invoke(this, e);
            }
        }
    }
}
