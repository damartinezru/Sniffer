using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkSniffer.Model
{
    /// <summary>
    /// Esta clase contiene metodos para abrir o cerrar la sesion de captura de datos
    /// </summary>
    class InterfaceMonitor
    {
        #region Campos
        private const uint MTU = 1024 * 64;
        private byte[] byteBufferData;
        private Socket socket;
        private IPAddress ipAddress;
        #endregion

        #region Constructor

        /// <param name="ip">Direccion IP en donde los paquetes necesitan ser capturados</param>
        public InterfaceMonitor(string ip)
        {
            byteBufferData = new byte[MTU];
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            ipAddress = IPAddress.Parse(ip);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Abre un nuevo socket y empieza a recibir informacion
        /// </summary>
        public void StartCapture()
        {
            /* Union del socket a la direccion ip seleccionada */
            socket.Bind(new IPEndPoint(ipAddress, 0));

            /* Opciones de socket aplicadas solo a los paquetes ip */
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] byteTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byteOut = new byte[4];
            /* Recibe todo lo que implica paquetes entrantes y de salida en la interfaz que esta siendo capturada.
            */
            socket.IOControl(IOControlCode.ReceiveAll, byteTrue, byteOut);

            byteBufferData = new byte[MTU];
            socket.BeginReceive(byteBufferData, 0, byteBufferData.Length,
                                SocketFlags.None, new AsyncCallback(this.ReceiveData), null);
        }

        /// <summary>
        /// Usada para recibir y procesar cada nuevo paquete y recibir el siguiente
        /// </summary>
        private void ReceiveData(IAsyncResult asyncResult)
        {
            try
            {
                int bytesReceived = socket.EndReceive(asyncResult);

                byte[] receivedData = new byte[bytesReceived];
                Array.Copy(byteBufferData, 0, receivedData, 0, bytesReceived);

                IPPacket newPacket = new IPPacket(receivedData, bytesReceived);
                if (newPacketEventHandler != null)
                {
                    newPacketEventHandler(newPacket);
                }
                
                socket.BeginReceive(byteBufferData, 0, byteBufferData.Length,
                                    SocketFlags.None, new AsyncCallback(this.ReceiveData), null);
            }
            catch
            {
                StopCapture();
            }

        }

        /// <summary>
        /// Cierra la sesion actual por medio del cierre de socket
        /// </summary>
        public void StopCapture()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
                ipAddress = null;
            }
        }
        #endregion

        #region handlers de eventos
        public event NewPacketEventHandler newPacketEventHandler;

        public delegate void NewPacketEventHandler(IPPacket newPacket);
        #endregion
    }
}
