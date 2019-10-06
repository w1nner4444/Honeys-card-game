using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Get8Backbone;
using NetworkController;

namespace ClientController
{
    /// <summary>
    /// This class is responsible for parsing the information
    /// from the network controller, updating the model, and informing the
    /// view that the world has changed.
    /// </summary>
    public class Controller
    {

        /// <summary>
        /// playerName initialized by ServerConnect and utilized in the FirstContact function
        /// </summary>
        private string playerName;

        //Player ID, just to have it (?) 
        private int playerID;


        /// <summary>
        /// The model of the world.
        /// </summary>
        private GameState game;

        /// <summary>
        /// Delegate used to do something if there is a networking error. 
        /// </summary>
        /// <param name="e"></param>
        public delegate void NetworkingError();

        public event NetworkingError connectionFailure;

        /// <summary>
        ///Boolean variables used to delegate strings being sent by the server during RecieveStartup. 
        /// </summary>
        private bool IDInit = false;
        private bool WorldInit = false;

        /// <summary>
        /// Determines if we can change the delegate of NetworkFunction within the SocketState
        /// </summary>

        /// <summary>
        /// Delegate used for the event of the button click to connect the client to the server.
        /// </summary>
        /// <param name="server"></param>
        public delegate void AttemptConnection();
        /// <summary>
        /// Delegate used for the event of a server message being sent, and deserialized.
        /// </summary>
        /// <param name="world"></param>
        public delegate void ServerUpdateHandler();
        /// <summary>
        /// Delegate used for the even to change the world size when recieved. 
        /// </summary>
        /// <param name="size"></param>
        public delegate void GameHandler();
        /// <summary>
        /// Event called when the button is clicked to connect the client to the server.
        /// </summary> 

        public delegate void TurnHandler(string turn); 

        public event AttemptConnection AttemptConnect;

        public event ServerUpdateHandler ServerUpdater;

        public event GameHandler SetupGame;

        public event TurnHandler TurnRecall;

        private Socket socket;

        public GameState Game { get => game; set => game = value; }

        public Controller()
        {
            game = null;
            Networking.connectionFailed += this.connectionFailed;

        }

        /// <summary>
        /// This is the method that should be called whenever a connection to the server has failed.
        /// </summary>
        private void connectionFailed()
        {
            connectionFailure();
        }

        public void ServerConnect(string hostName, string playerName)
        {
            this.playerName = playerName;
            System.Diagnostics.Debug.Write("About to initiate connection to server " + hostName + "..../n");
            socket = Networking.ConnectToServer(FirstContact, hostName);
            //System.Diagnostics.Debug.Write("Finished attempting a server connection.");
            AttemptConnect();
        }

        /// <summary>
        /// Sends the player's name and a new line to server. (Based upon custom networking protocols)
        /// </summary>
        /// <param name="s"></param>
        public void FirstContact(SocketState s)
        {
            s.NetworkFunction = ReceiveStartup;
            Networking.Send(s.TheSocket, playerName);
            System.Diagnostics.Debug.WriteLine("...End invoking networking function");
        }

        /// <summary>
        /// First call when we have recieved the startup data. 
        /// </summary>
        /// <param name="m"></param>
        private void ReceiveStartup(SocketState m)
        {
            ProcessMessage(m, true);
            if (WorldInit)
            {
                m.NetworkFunction = ReceiveWorld;
            }
            Networking.GetData(m);
        }

        private void ReceiveWorld(SocketState m)
        {
            ProcessMessage(m, false);
            this.SendData(m.TheSocket);
            Networking.GetData(m);
        }

        private void SendData(Socket m)
        {
            
                Networking.Send(m, "RANDOM");
        }

        /// <summary>
        /// This is the worker method that processes a message when received.
        /// For proper MVC, this should go in its own controller
        /// </summary>
        /// <param name="ss">The SocketState on which the message was received</param>
        private void ProcessMessage(SocketState ss, bool isRecieveStartup)
        {
            string totalData = ss.MessageState.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                //Recieve startup is a special first case called on process message. 
                //When this is true, the client will attempt to determine the world size and extract the player ID. 
                if (isRecieveStartup)
                {

                    //If the ID has not been extracted.
                    if (!IDInit)
                    { 
                        //We have extracted some ID. It is not important necessarily to store this ID
                        if (Int32.TryParse(p, out int playerID))
                        {
                            IDInit = true;
                        }

                    }
                    //If the world size has not been extracted. 
                    else if (!WorldInit)
                    {
                        if (Int32.TryParse(p, out int worldSize))
                        {
                            SetupGame();

                            WorldInit = true;
                        }

                    }

                }
                //If it is not this special first case, the client will attempt to deserialize each completed 
                //message. 
                else
                {
                    lock (this.Game)
                    {
                        TurnRecall(p);
                    }


                }


                // Then remove it from the SocketState's growable buffer

                ss.MessageState.Remove(0, p.Length);
            }
            //We only want to update the server every frame which occurs at the end of all the processed messages. 
            ServerUpdater();
        }
    }

    //Initializes the Game World According to input. 
    public void InitGameState(string p)
    {

    }
}
