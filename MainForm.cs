using System;
using System.Drawing;
using System.Windows.Forms;
using Unidad1.MemoriaDinamica;
using Unidad1.MemoriaEstatica;

namespace Unidad1
{
    /// <summary>
    /// Formulario de la Tarea 1.2. Pestaña 1: vector con memoria estática y
    /// redimensionamiento manual. Pestaña 2: lista enlazada con memoria dinámica.
    /// </summary>
    public class MainForm : Form
    {
        private VectorEstatico _vector = new VectorEstatico(4);
        private readonly ListaEnlazada _lista = new ListaEnlazada();

        private readonly TabControl tabs = new TabControl { Dock = DockStyle.Fill };

        // Controles de la pestaña del vector.
        private readonly TextBox txtVectorValor = new TextBox { Location = new Point(20, 20), Width = 120 };
        private readonly Button btnAgregar = new Button { Text = "Agregar", Location = new Point(150, 18), Width = 120 };
        private readonly TextBox txtVectorIndice = new TextBox { Location = new Point(20, 60), Width = 120 };
        private readonly Button btnEliminar = new Button { Text = "Eliminar en índice", Location = new Point(150, 58), Width = 140 };
        private readonly Button btnReiniciar = new Button { Text = "Reiniciar (capacidad 4)", Location = new Point(20, 100), Width = 200 };
        private readonly Label lblVectorEstado = new Label { Location = new Point(20, 140), Size = new Size(640, 40) };
        private readonly TextBox txtVectorLog = new TextBox
        {
            Location = new Point(20, 190),
            Size = new Size(640, 220),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical
        };

        // Controles de la pestaña de la lista enlazada.
        private readonly TextBox txtListaValor = new TextBox { Location = new Point(20, 20), Width = 120 };
        private readonly Button btnInicio = new Button { Text = "Insertar al inicio", Location = new Point(20, 60), Width = 150 };
        private readonly Button btnFinal = new Button { Text = "Insertar al final", Location = new Point(180, 60), Width = 150 };
        private readonly Button btnEliminarLista = new Button { Text = "Eliminar valor", Location = new Point(340, 60), Width = 150 };
        private readonly Label lblLista = new Label { Location = new Point(20, 110), Size = new Size(640, 25) };
        private readonly Button btnReferencias = new Button { Text = "Demostrar referencias en el Heap", Location = new Point(20, 160), Width = 280 };
        private readonly Button btnHeap = new Button { Text = "Medir Heap (100 000 nodos)", Location = new Point(20, 200), Width = 280 };
        private readonly Label lblSalidaLista = new Label { Location = new Point(20, 245), Size = new Size(640, 150) };

        public MainForm()
        {
            Text = "Tarea 1.2 - Memoria estática vs. memoria dinámica";
            ClientSize = new Size(700, 480);
            StartPosition = FormStartPosition.CenterScreen;

            ConstruirPestanaVector();
            ConstruirPestanaLista();
            Controls.Add(tabs);

            ActualizarVector();
            ActualizarLista();
        }

        // ---------- Pestaña 1: memoria estática ----------

        private void ConstruirPestanaVector()
        {
            var pagina = new TabPage("Memoria estática (vector)");
            pagina.Controls.AddRange(new Control[]
            {
                txtVectorValor, btnAgregar, txtVectorIndice, btnEliminar,
                btnReiniciar, lblVectorEstado, txtVectorLog
            });
            tabs.TabPages.Add(pagina);

            btnAgregar.Click += (s, e) => AgregarAlVector();
            btnEliminar.Click += (s, e) => EliminarDelVector();
            btnReiniciar.Click += (s, e) => ReiniciarVector();
        }

        private void AgregarAlVector()
        {
            if (!int.TryParse(txtVectorValor.Text, out int valor))
            {
                MessageBox.Show(this, "Ingrese un entero válido.", "Validación");
                return;
            }

            int capacidadAntes = _vector.Capacidad;
            _vector.Agregar(valor);

            // Se registra el redimensionamiento real cuando ocurre.
            if (_vector.Capacidad != capacidadAntes)
            {
                RegistrarVector($"  -> Redimensionado: {capacidadAntes} => {_vector.Capacidad}");
            }
            RegistrarVector($"Agregado {valor}");

            txtVectorValor.Clear();
            ActualizarVector();
        }

        private void EliminarDelVector()
        {
            if (!int.TryParse(txtVectorIndice.Text, out int indice))
            {
                MessageBox.Show(this, "Ingrese un índice entero válido.", "Validación");
                return;
            }

            try
            {
                _vector.EliminarEn(indice);
                RegistrarVector($"Eliminado el elemento del índice {indice}");
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(this, ex.Message, "Índice fuera de rango");
            }
            ActualizarVector();
        }

        private void ReiniciarVector()
        {
            _vector = new VectorEstatico(4);
            txtVectorLog.Clear();
            ActualizarVector();
        }

        private void RegistrarVector(string mensaje)
        {
            txtVectorLog.AppendText(mensaje + Environment.NewLine);
        }

        private void ActualizarVector()
        {
            lblVectorEstado.Text =
                $"Contenido: {_vector}   |   Cantidad: {_vector.Cantidad}   |   Capacidad: {_vector.Capacidad}";
        }

        // ---------- Pestaña 2: memoria dinámica ----------

        private void ConstruirPestanaLista()
        {
            var pagina = new TabPage("Memoria dinámica (lista enlazada)");
            pagina.Controls.AddRange(new Control[]
            {
                txtListaValor, btnInicio, btnFinal, btnEliminarLista,
                lblLista, btnReferencias, btnHeap, lblSalidaLista
            });
            tabs.TabPages.Add(pagina);

            btnInicio.Click += (s, e) => InsertarEnLista(alInicio: true);
            btnFinal.Click += (s, e) => InsertarEnLista(alInicio: false);
            btnEliminarLista.Click += (s, e) => EliminarDeLista();
            btnReferencias.Click += (s, e) => DemostrarReferencias();
            btnHeap.Click += (s, e) => MedirHeap();
        }

        private void InsertarEnLista(bool alInicio)
        {
            if (!int.TryParse(txtListaValor.Text, out int valor))
            {
                MessageBox.Show(this, "Ingrese un entero válido.", "Validación");
                return;
            }

            if (alInicio) _lista.InsertarAlInicio(valor);
            else _lista.InsertarAlFinal(valor);

            txtListaValor.Clear();
            ActualizarLista();
        }

        private void EliminarDeLista()
        {
            if (!int.TryParse(txtListaValor.Text, out int valor))
            {
                MessageBox.Show(this, "Ingrese un entero válido.", "Validación");
                return;
            }

            bool eliminado = _lista.Eliminar(valor);
            if (!eliminado)
            {
                MessageBox.Show(this, $"El valor {valor} no está en la lista.", "No encontrado");
            }
            ActualizarLista();
        }

        private void DemostrarReferencias()
        {
            // Dos variables apuntan al mismo objeto en el Heap.
            Nodo a = new Nodo(100);
            Nodo b = a;
            b.Valor = 200;

            bool mismaInstancia = ReferenceEquals(a, b);
            string texto =
                $"a.Valor = {a.Valor} | b.Valor = {b.Valor} | ¿Misma instancia en el Heap? {mismaInstancia}.";

            // Al anular ambas referencias, el objeto queda elegible para el Garbage Collector.
            a = null!;
            b = null!;
            lblSalidaLista.Text = texto + Environment.NewLine +
                "Referencias anuladas: el objeto queda elegible para el GC.";
        }

        private void MedirHeap()
        {
            long antes = GC.GetTotalMemory(forceFullCollection: true);
            var masNodos = new ListaEnlazada();
            for (int i = 0; i < 100_000; i++)
            {
                masNodos.InsertarAlInicio(i);
            }
            long despues = GC.GetTotalMemory(forceFullCollection: false);

            lblSalidaLista.Text =
                $"Heap aproximado para 100 000 nodos: {(despues - antes) / 1024} KB." + Environment.NewLine +
                "Es una estimación: depende del estado del Garbage Collector.";
        }

        private void ActualizarLista()
        {
            lblLista.Text = $"Lista: {_lista}   |   Tamaño: {_lista.Tamanio}";
        }
    }
}
