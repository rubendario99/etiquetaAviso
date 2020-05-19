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
        Circulo
    }

    public partial class EtiquetaAviso : Control
    {

        public EtiquetaAviso()
        {
            InitializeComponent();
        }

        private Color colorInicioGradiente;
        [Category("Appareance")]
        [Description("Color inicial del gradiente")]
        public Color ColorInicioGradiente
        {
            set
            {
                colorInicioGradiente = value;
                // Refresh();
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
                // Refresh();
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

                e.Graphics.DrawLine(pen, 0, 0, this.Location.X, 0);
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
            }
            //Finalmente pintamos el Texto; desplazado si fuera necesario
            SolidBrush b = new SolidBrush(this.ForeColor);
            g.DrawString(this.Text, this.Font, b, offsetX + grosor, offsetY);
            Size tam = g.MeasureString(this.Text, this.Font).ToSize();
            this.Size = new Size(tam.Width + offsetX + grosor, tam.Height + offsetY * 2);
            b.Dispose();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh();
        }
    }
}
