using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace NetworkSniffer.Model
{

    public class TCPPacket
    {
        #region Campos
        private const uint TCPHeaderSize = 20;
        private byte[] byteTCPHeader = new byte[TCPHeaderSize];
        private byte[] byteTCPMessage;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicializa la  instancia de la clase TCPPacket 
        /// </summary>
        /// <param name="byteBuffer">Byte array conteniendo la data del paquete</param>
        /// <param name="length">Tamano de paquete en bytes</param>
        public TCPPacket(byte[] byteBuffer, int length)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byteBuffer, 0, length);
                
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                
                Array.Copy(byteBuffer, byteTCPHeader, TCPHeaderSize);
                
                byteTCPMessage = new byte[length - TCPHeaderSize];
                Array.Copy(byteBuffer, TCPHeaderSize, byteTCPMessage, 0, length - TCPHeaderSize);

                TCPHeader = new List<TCPHeader>();
                DNSPacket = new List<DNSPacket>();

                PopulatePacketContents();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Parte del header de TCPPacket
        /// </summary>
        public List<TCPHeader> TCPHeader { get; set; }

        /// <summary>
        /// Mantiene el mensaje TCP si el protocolo de la aplicacion es DNS
        /// </summary>
        public List<DNSPacket> DNSPacket { get; set; }

        /// <summary>
        /// Informacion del protocolo
        /// </summary>
        public ApplicationProtocolType ApplicationProtocolType { get; private set; }

        /// <summary>
        /// Coleccion que almacena header y mensaje
        /// </summary>
        public IList PacketContent
        {
            get
            {
                return new CompositeCollection()
                {
                    new CollectionContainer() { Collection = TCPHeader },
                    new CollectionContainer() { Collection = DNSPacket }
                };
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Pone el contenido de paquete en una lista de contenido de paquetes
        /// Agregar la informacion del header en una lista de tipo TCPHeader
        /// </summary>
        private void PopulatePacketContents()
        {
            TCPHeader.Add(new TCPHeader(byteTCPHeader, (int)TCPHeaderSize));
            
            if (TCPHeader[0].DestinationPort == 53)
            {
                DNSPacket.Add(new DNSPacket(byteTCPMessage, byteTCPMessage.Length));
            }
            else if (TCPHeader[0].SourcePort == 53)
            {
                DNSPacket.Add(new DNSPacket(byteTCPMessage, byteTCPMessage.Length));
            }

            ApplicationProtocolType = new ApplicationProtocolType(TCPHeader[0].SourcePort,
                                                                  TCPHeader[0].DestinationPort);
        }
        #endregion
    }
}
