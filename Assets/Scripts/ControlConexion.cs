using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    [Header ("Paneles")]
    [SerializeField] private GameObject panelConectar;
    [SerializeField] private GameObject panelBienvenida;
    [SerializeField] private GameObject panelCrearSala;
    [SerializeField] private GameObject panelConectarSala;
    [SerializeField] private GameObject panelSalaJuego;

    [Header("Botones")]
    [SerializeField] private Button conectarBtn;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField inputNombreJugador;
    [SerializeField] private TextMeshProUGUI txtBienvenida;
    [SerializeField] private TextMeshProUGUI txtBarraEstado;

    [Header ("Panel creación de sala")]
    [SerializeField] private Button btnCrearNuevaSala;
    [SerializeField] private TMP_InputField txtNombreSala;
    [SerializeField] private TMP_InputField txtMinJugadores;
    [SerializeField] private TMP_InputField txtMaxJugadores;

    [Header("Panel Unirse a una sala")]
    [SerializeField] private TMP_InputField txtNombreSalaUnirse;
    [SerializeField] private Button btnUnirseSala;

    //[Header("Panel Sala de Juego")]
    //[SerializeField] private 

    void Start()
    {
        ActivarPaneles(panelConectar);
    }

    private void Estado(string _mensaje)
    {
        txtBarraEstado.text = _mensaje;

    }

    #region CODIGO_BOTONES
    public void Pulsar_BtnConectar()
    {
        if(!string.IsNullOrEmpty(inputNombreJugador.text) && !string.IsNullOrWhiteSpace(inputNombreJugador.text))
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = inputNombreJugador.text;
            Estado("Conectando a Photon");
        }
        else
        {
            Estado("Introduzca un nombre de jugador válido");
        }
    }

    public void Pulsar_BtnCrearNuevaSala()
    {
        ActivarPaneles(panelCrearSala);

        Estado("Creando nueva sala");
    }

    public void Pulsar_BtnConectarSala()
    {
        ActivarPaneles(panelConectarSala);

        Estado("Uniéndose a sala");
    }

    public void Pulsar_BtnUnirseSala()
    {
        if (!string.IsNullOrEmpty(txtNombreSalaUnirse.text) && !string.IsNullOrWhiteSpace(txtNombreSalaUnirse.text))
        {
            PhotonNetwork.JoinRoom(txtNombreSalaUnirse.text);
        }
        else
        {
            Estado("Introduzca nombre de sala correcto");
        }
    }

    public void Pulsar_BtnCreacionNuevaSala()
    {
        byte minJugadores;
        byte maxJugadores;
        minJugadores = byte.Parse(txtMinJugadores.text);
        maxJugadores = byte.Parse(txtMaxJugadores.text);
        
        if(!string.IsNullOrEmpty(txtNombreSala.text) && !string.IsNullOrWhiteSpace(txtNombreSala.text))
        {
            if ((minJugadores <= maxJugadores) && maxJugadores <= 20 && minJugadores >= 2)
            {
                RoomOptions opcionesSala = new RoomOptions();

                opcionesSala.MaxPlayers = maxJugadores;
                //Para poder unirse posteriormente desde Conectarse a Sala tenemos que hacerla visible
                opcionesSala.IsVisible = true;

                PhotonNetwork.CreateRoom(txtNombreSala.text, opcionesSala, TypedLobby.Default);
                Estado("Creando la nueva sala: " + txtNombreSala.text);
            }
            else {
                Estado("Introduzca valores correctos");
            }
        }
        else
        {
            Estado("Introduzca un nombre de sala correcto");
        }
    }
    /// <summary>
    /// Salir del juego
    /// </summary>
    public void Pulsar_Salir()
    {
        Application.Quit();
    }
    /// <summary>
    /// Estamos en la pantalla de bienvenida, con lo cual 
    /// debemos desconectar Photon y volver a la pantalla Conectar
    /// </summary>
    public void Pulsar_Desconectar()
    {
        PhotonNetwork.Disconnect();
        ActivarPaneles(panelConectar);
    }
    /// <summary>
    /// Estamos en la pantalla de Creación de sala y queremos
    /// volver a la pantalla de Bienvenida 
    /// </summary>
    public void Pulsar_Volver()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        ActivarPaneles(panelBienvenida);
    }
    #endregion

    #region callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Estado("Conectado a Photon");
        ActivarPaneles(panelBienvenida);

        txtBienvenida.text = "Bienvenido/a " + PhotonNetwork.NickName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        Estado("Desconectado de Photon " + cause);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Estado("No se ha podido crear la sala." + message);
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        string mensaje = PhotonNetwork.NickName + " se ha conectado a " + PhotonNetwork.CurrentRoom.Name;

        Estado(mensaje);

        ActivarPaneles(panelSalaJuego);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        Estado("No se ha podido unir a la sala." + message);

    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        string mensaje = PhotonNetwork.NickName + " se ha unido a " + PhotonNetwork.CurrentRoom.Name;

        Estado(mensaje);

        ActivarPaneles(panelSalaJuego);
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        Estado("Abandonaste la sala");
    }
    #endregion

    private void ActivarPaneles(GameObject _panel)
    {
        panelConectar.SetActive(false);
        panelBienvenida.SetActive(false);
        panelCrearSala.SetActive(false);
        panelConectarSala.SetActive(false);
        panelSalaJuego.SetActive(false);

        _panel.SetActive(true);
    }

}
