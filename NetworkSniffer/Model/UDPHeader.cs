using System.IO;
using System.Net;

namespace NetworkSniffer.Model
{
    /// <summary>
    /// Esta clase es utilizada para parsear y asignar los campos de los headers UDP
    /// </summary>
    public class UDPHeader
    {
        #region Constructor
        /// <summary>
        /// Inicializacion de instancia de la clase
        /// </summary>
        /// <param name="byteBuffer">Arreglo de bytes que contiene la data del header</param>
        /// <param name="length">Tamano del header</param>
        public UDPHeader(byte[] byteBuffer, int length)
        {
            MemoryStream memoryStream = new MemoryStream(byteBuffer, 0, length);

            BinaryReader binaryReader = new BinaryReader(memoryStream);

            SourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            DestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            Length = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            Checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        }
        #endregion

        #region Properties
        public ushort SourcePort { get; set; }

        public ushort DestinationPort { get; set; }

        public ushort Length { get; set; }

        public short Checksum { get; set; }
        #endregion
    }
}
