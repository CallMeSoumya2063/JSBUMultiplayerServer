﻿using JustShapesBeatsMultiplayerServer.Controllers;
using JustShapesBeatsMultiplayerServer.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace JustShapesBeatsMultiplayerServer.Managers
{
    class ServerManager
    {
        public bool IsListening
        {
            get => _tcpController != null && _tcpController.IsListening;
        }

        private TcpController _tcpController;
        private UdpController _udpController;

        private ClientManager _clientManager;
        private RoomManager _roomManager;

        public void Init()
        {
            Debug.Log("Init server...");
            Debug.Log($"Game Version: {Constants.GameVersion}");
            Debug.Log($"Support build: {Constants.GameSupportBuild}");
            Debug.Log($"Tcp port: {Constants.GamePort}");
            Debug.Log($"Max players: {Constants.MaxPlayers}");

            PacketHandler.Init();

            _roomManager = new RoomManager();
            _clientManager = new ClientManager();
            _tcpController = new TcpController(Constants.GamePort, ref _clientManager);
            _udpController = new UdpController(Constants.GamePort, ref _clientManager);

            _roomManager.SetClientManager(ref _clientManager);
            _clientManager.SetRoomManager(ref _roomManager);
        }

        public void Start()
        {
            Debug.Log("Start server...");

            _tcpController.Start();
            _udpController.Start();

            _clientManager.StartPingClients();
            _clientManager.StartCheckPlayersAndRooms();
        }

        public void Stop()
        {
            _tcpController.Stop();
            _udpController.Stop();
        }

        public ClientManager GetClientManager()
        {
            return _clientManager;
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }

        public void SendBytesByUdpProtocol(IPEndPoint endPoint, byte[] bytes)
        {
            _udpController.SendBytes(endPoint, bytes);
        }
    }
}
