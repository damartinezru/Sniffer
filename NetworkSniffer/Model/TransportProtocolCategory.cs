using System;
using System.ComponentModel;

namespace NetworkSniffer
{
    /// <summary>
    /// Clase usada para almacenar estadisticas de un protocolo det trasnporte particular
    /// </summary>
    public class TransportProtocolCategory : INotifyPropertyChanged
    {
        #region Constructores

        /// <param name="protocolName">Nombre de la categoria de protocolo</param>
        public TransportProtocolCategory(string protocolName)
        {
            ProtocolName = protocolName;
        }


        /// <param name="count">Numero de paquetes que contienen paquetes de trasnporte que usan un protocolo especifico </param>
        /// <param name="percentage">Porcentaje de paquetes que contienen paquetes de transporte que usan un protocolo especifico</param>
        public TransportProtocolCategory(int count, double percentage)
        {
            Count = count;
            Percentage = percentage;
        }


        public TransportProtocolCategory(string protocolName, int count, double percentage)
            : this(count, percentage)
        {
            ProtocolName = protocolName;
        }
        #endregion

        #region Propiedades

        public string ProtocolName { get; set; }

        private int count;
        /// <summary>
        /// Numero de paquetes que contiene paquetes de transporte que usa un protocolo especifico
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NotifyPropertyChanged("Count");
            }
        }

        private double percentage;
        /// <summary>
        /// Porcentaje de paquetes que contienen paquetes de transporte que usan un protocolo especifico
        /// </summary>
        public double Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                percentage = Math.Round(value, 3);
                NotifyPropertyChanged("Percentage");
            }
        }
        #endregion

        #region handlers de evento
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
