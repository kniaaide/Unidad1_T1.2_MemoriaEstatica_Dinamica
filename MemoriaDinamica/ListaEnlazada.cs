using System;
using System.Text;

namespace Unidad1.MemoriaDinamica
{
    /// <summary>
    /// Lista simplemente enlazada. Su tamaño crece y decrece en tiempo de
    /// ejecución sin reasignar bloques contiguos.
    /// </summary>
    public class ListaEnlazada
    {
        private Nodo? _cabeza;   // Referencia al primer nodo
        private int _tamanio;

        public int Tamanio => _tamanio;

        /// <summary>Inserta al inicio. Costo O(1).</summary>
        public void InsertarAlInicio(int valor)
        {
            var nuevo = new Nodo(valor);   // Instancia en el Heap
            nuevo.Siguiente = _cabeza;     // Enlace hacia el anterior primero
            _cabeza = nuevo;               // Actualiza la referencia de cabeza
            _tamanio++;
        }

        /// <summary>Inserta al final. Costo O(n) por el recorrido.</summary>
        public void InsertarAlFinal(int valor)
        {
            var nuevo = new Nodo(valor);

            if (_cabeza == null)
            {
                _cabeza = nuevo;
            }
            else
            {
                Nodo actual = _cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevo;
            }
            _tamanio++;
        }

        /// <summary>Elimina la primera aparición del valor. Retorna true si se eliminó.</summary>
        public bool Eliminar(int valor)
        {
            if (_cabeza == null) return false;

            // Caso: el valor está en la cabeza.
            if (_cabeza.Valor == valor)
            {
                _cabeza = _cabeza.Siguiente;   // El nodo anterior queda sin referencias
                _tamanio--;
                return true;
            }

            Nodo actual = _cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Valor != valor)
            {
                actual = actual.Siguiente;
            }

            if (actual.Siguiente == null) return false;

            actual.Siguiente = actual.Siguiente.Siguiente;   // Desenlace del nodo
            _tamanio--;
            return true;
        }

        /// <summary>Representación textual de la lista.</summary>
        public override string ToString()
        {
            var sb = new StringBuilder("[ ");
            Nodo? actual = _cabeza;
            while (actual != null)
            {
                sb.Append(actual.Valor);
                if (actual.Siguiente != null) sb.Append(" -> ");
                actual = actual.Siguiente;
            }
            return sb.Append(" ]").ToString();
        }
    }
}
