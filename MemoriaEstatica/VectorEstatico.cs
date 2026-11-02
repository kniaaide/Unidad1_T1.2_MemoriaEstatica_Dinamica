using System;

namespace Unidad1.MemoriaEstatica
{
    /// <summary>
    /// Vector basado en un arreglo de tamaño fijo. Cuando se alcanza la
    /// capacidad, se reserva manualmente un nuevo bloque contiguo, se copian
    /// los elementos y se libera el bloque anterior.
    /// </summary>
    public class VectorEstatico
    {
        // Arreglo de capacidad fija. Su longitud es la capacidad real.
        private int[] _datos;

        // Número de posiciones ocupadas. Invariante: 0 <= _cantidad <= _datos.Length
        private int _cantidad;

        /// <summary>Crea el vector con una capacidad inicial.</summary>
        public VectorEstatico(int capacidadInicial)
        {
            if (capacidadInicial <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacidadInicial),
                    "La capacidad inicial debe ser mayor que cero.");

            _datos = new int[capacidadInicial];   // Asignación en el Heap, tamaño fijo
            _cantidad = 0;
        }

        /// <summary>Capacidad total reservada.</summary>
        public int Capacidad => _datos.Length;

        /// <summary>Número de elementos almacenados.</summary>
        public int Cantidad => _cantidad;

        /// <summary>Acceso directo por índice en tiempo O(1).</summary>
        public int this[int indice]
        {
            get
            {
                ValidarIndice(indice);
                return _datos[indice];
            }
            set
            {
                ValidarIndice(indice);
                _datos[indice] = value;
            }
        }

        /// <summary>Agrega un elemento al final. Redimensiona si está lleno.</summary>
        public void Agregar(int valor)
        {
            if (_cantidad == _datos.Length)
            {
                // Estrategia de duplicación: amortiza el costo de la copia.
                Redimensionar(_datos.Length * 2);
            }
            _datos[_cantidad] = valor;
            _cantidad++;
        }

        /// <summary>
        /// Redimensionamiento manual: reserva un nuevo arreglo, copia los
        /// datos vigentes y reasigna la referencia. El arreglo anterior queda
        /// sin referencias y será recolectado por el Garbage Collector.
        /// </summary>
        public void Redimensionar(int nuevaCapacidad)
        {
            if (nuevaCapacidad < _cantidad)
                throw new ArgumentException(
                    "La nueva capacidad no puede ser menor que la cantidad de elementos.");

            int[] nuevoBloque = new int[nuevaCapacidad];   // Nuevo bloque contiguo

            for (int i = 0; i < _cantidad; i++)            // Copia elemento a elemento: O(n)
            {
                nuevoBloque[i] = _datos[i];
            }

            _datos = nuevoBloque;                          // Reasignación de la referencia
        }

        /// <summary>Elimina el elemento en el índice indicado desplazando a la izquierda.</summary>
        public void EliminarEn(int indice)
        {
            ValidarIndice(indice);

            for (int i = indice; i < _cantidad - 1; i++)   // Desplazamiento: O(n)
            {
                _datos[i] = _datos[i + 1];
            }

            _cantidad--;
            _datos[_cantidad] = 0;                         // Limpia la posición liberada
        }

        /// <summary>Devuelve una representación textual de los elementos.</summary>
        public override string ToString()
        {
            if (_cantidad == 0) return "[ ]";

            string resultado = "[ ";
            for (int i = 0; i < _cantidad; i++)
            {
                resultado += _datos[i];
                if (i < _cantidad - 1) resultado += ", ";
            }
            return resultado + " ]";
        }

        private void ValidarIndice(int indice)
        {
            if (indice < 0 || indice >= _cantidad)
                throw new IndexOutOfRangeException(
                    $"Índice {indice} fuera de rango. Cantidad actual: {_cantidad}.");
        }
    }
}
