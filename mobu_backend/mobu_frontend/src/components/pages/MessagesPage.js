import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import React, { useCallback, useLayoutEffect, useEffect, useRef, useState } from "react";
import TabHeader from "../modular/TabHeader";
import TabPanel from "../modular/TabPanel";
import MessageHeaderBar from "../single_use/messageHeaderBar/MessageHeaderBar";
import MessageFooterBar from "../single_use/messageFooterBar/MessageFooterBar";
import MessagePanel from "../single_use/messagePanel/MessagePanel";

/**
 * 
 * Pagina de mensagens
 * 
 * @returns 
 */
export default function MessagesPage() {

    const queryParamId = new URLSearchParams(window.location.search).get('id');

    const owner = useRef();
    const connection = useRef();

    const [hasFetchedData, setHasFetchedData] = useState(false);
    const [selectedFriendItem, setSelectedFriendItem] = useState(0);
    const [selectedGroupItem, setSelectedGroupItem] = useState(0);
    const [lastMessageReceived, setLastMessageReceived] = useState({
        ItemId: -1,
        IsFriends: true,
        Message:
        {
            IDMensagem: -1,
            IDRemetente: -1,
            URLImagemRemetente: "",
            NomeRemetente: "",
            ConteudoMsg: ""
        }
    });
    const [friendsTab, setFriendsTab] = useState(true);
    const [friendsData, setFriendsData] = useState([
        {
            ItemId: -1,
            FriendId: -1,
            FriendName: "",
            CommonRoomId: -1,
            ImageURL: "",
            Messages: new Array(
                {
                    IDMensagem: -1,
                    IDRemetente: -1,
                    URLImagemRemetente: "",
                    NomeRemetente: "",
                    ConteudoMsg: ""
                }
            )
        }
    ]);
    const [groupsData, setGroupsData] = useState([
        {
            ItemId: -1,
            IDSala: -1,
            NomeSala: "",
            ImageURL: "",
            Mensagens: new Array(
                {
                    IDMensagem: -1,
                    IDRemetente: -1,
                    URLImagemRemetente: "",
                    NomeRemetente: "",
                    ConteudoMsg: ""
                }
            )
        }
    ]);
    
    useEffect(() => {


        //debugger;
        if (hasFetchedData) {
            // atualizar dados das mensagens de amigos do utilizador
            var aux = {};
            var trailing = [];
            var leading = [];
            var isGroup = !lastMessageReceived.IsFriends;
            var itemId = lastMessageReceived.ItemId;
            var messageObject = lastMessageReceived.Message;

            if (!isGroup) {
                aux = { ...friendsData[itemId] };
                aux.Messages.push({ ...messageObject });
                if (itemId === 0) {

                    trailing = friendsData.slice(1, friendsData.length);

                    setFriendsData([
                        { ...aux },
                        ...trailing
                    ]);
                } else if (itemId === friendsData.length - 1) {

                    leading = friendsData.slice(0, friendsData.length - 1);

                    setFriendsData([
                        ...leading,
                        { ...aux }
                    ]);
                } else {

                    leading = friendsData.slice(0, itemId);
                    trailing = friendsData.slice(itemId + 1, friendsData.length)

                    setFriendsData([
                        ...leading,
                        { ...aux },
                        ...trailing
                    ]);
                }
            } else {
                aux = { ...groupsData[itemId] };
                aux.Mensagens.push({ ...messageObject });
                if (itemId === 0) {

                    trailing = groupsData.slice(1, friendsData.length);

                    setGroupsData([
                        { ...aux },
                        ...trailing
                    ]);
                } else if (itemId === friendsData.length - 1) {

                    leading = groupsData.slice(0, friendsData.length - 1);

                    setGroupsData([
                        ...leading,
                        { ...aux }
                    ]);
                } else {

                    leading = groupsData.slice(0, itemId);
                    trailing = groupsData.slice(itemId + 1, friendsData.length)

                    setGroupsData([
                        ...leading,
                        { ...aux },
                        ...trailing
                    ]);
                }
            }
        }
        else {

            var options = {
                method: "GET",
                redirect: "follow",
                credentials: "include"
            }

            var queryParams = `?id=${queryParamId}`;

            fetch(process.env.REACT_APP_API_URL + "/messages" + queryParams, options)
                .then(response => {
                    if (response.status === 404) {
                        window.location.assign("/error-404");
                    }
                    else if (response.status === 500) {
                        window.location.assign("/error-500");
                    }
                    else if (response.status === 403) {
                        window.location.assign("/error-403");
                    }
                    else if (response.status === 401) {
                        window.history.back();
                    }
                    return response.json();
                })
                .then(data => {

                    // dono
                    owner.current = data.ownerInfo;

                    // dados
                    //debugger;

                    setFriendsData([...data.friends]);
                    setGroupsData([...data.groups]);

                    setHasFetchedData(true);

                    connection.current.invoke("AddConnection", [...data.friends][0].CommonRoomId + "");
                })
                .catch(err => console.error("error: ", err));

            connection.current =
                new HubConnectionBuilder().withUrl(process.env.REACT_APP_HUB_URL + "/RealTimeHub", {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                    .configureLogging(LogLevel.Debug)
                    .build();
            //debugger;
            start(connection.current);
            logSignalRAccess(connection.current);
            listenToSignalRMessages(connection.current);
            listenToSignalRGroupChange(connection.current);

            setTimeout(() => {
                logout();
                window.location.assign("/");

            }, 15 * 1000 * 60);
        }
    }, [lastMessageReceived]);



    /**
     * ligacao ao hub signalR
     */
    const start = async (connection) => {
        try {
            //debugger;
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

    /**
     * funcao de logout
     */
    const logout = async() => {

        await connection.current.stop();

        var options = {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify({ Id: owner.current }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            },
            credentials: "include"
        }

        await fetch(process.env.REACT_APP_API_URL + "/logout", options)
            .then(response => {
                if (response.status === 404) {
                    window.location.assign("/error-404");
                }
                else if (response.status === 500) {
                    window.location.assign("/error-500");
                }
            })
            .catch(err => console.error("error: ", err));
    }

    /**
     * mensagem de log do signalR ao conectar
     * 
     * @param {any} connection
     */
    const logSignalRAccess = (connection) => {
        connection.on("OnConnectedAsyncPrivate", message => {
            console.log(message);
        });
    }

    /**
     * Colocar o listener para o metodo 'ReceiveMessage' vindo do hub signalR
     * e armazenar mensagens recebidas
     * 
     * @param {any} connection
     */
    const listenToSignalRMessages = (connection) => {
        connection.on("ReceiveMessage", (itemId, isGroup, messageObject) => {

            console.log("ReceiveMessage");

            setLastMessageReceived({
                ItemId: itemId,
                IsFriends: !isGroup,
                Message: {
                    IDMensagem: messageObject.idMensagem,
                    IDRemetente: messageObject.idRemetente,
                    URLImagemRemetente: messageObject.urlImagemRemetente,
                    NomeRemetente: messageObject.nomeRemetente,
                    ConteudoMsg: messageObject.conteudoMsg
                }
            });
            console.log("ReceiveMessage");
        })
    };

    /**
     * ouvir a mudanca entre salas para mudar o grupo do signalR
     * 
     * @param {any} connection
     */
    const listenToSignalRGroupChange = (connection) => {
        connection.on("AddedToGroup", (connectionId, roomId) => {
            console.log("conexão " + connectionId + " adicionada à sala " + roomId);
        });

        connection.on("RemovedFromGroup", (connectionId, roomId) => {
            console.log("conexão " + connectionId + " removida da sala " + roomId);
        });
    }

    /**
     * clique no separador dos amigos
     */
    const handleFriendsTabHeaderClick = () => {
        setFriendsTab(true)
    }

    /**
     * clique no separador dos grupos
     */
    const handleGroupsTabHeaderClick = () => {
        setFriendsTab(false)
    }

    /**
     * clique nos itens dos amigos
     */
    function handleFriendItemClick(itemKey) {
        setSelectedFriendItem(itemKey);
    }

    /**
     * clique nos itens dos grupos
     */
    function handleGroupItemClick(itemKey) {
        setSelectedGroupItem(itemKey);
    }

    return (
        <>
            {
            hasFetchedData ?
                <>
                    <div className="tabs-div">
                        <div className="tabs-headers-div">
                            <TabHeader
                                text="Amigos"
                                onHeaderClick={handleFriendsTabHeaderClick}
                                personGroupData={{
                                    friend: friendsData,
                                    group: groupsData
                                }}
                                selectedItem={{
                                    friend: selectedFriendItem,
                                    group: selectedGroupItem
                                }}
                                connection={connection.current}
                                isFriends={friendsTab}
                            />
                            <TabHeader
                                text="Grupos"
                                onHeaderClick={handleGroupsTabHeaderClick}
                                personGroupData={{
                                    friend: friendsData,
                                    group: groupsData
                                }}
                                selectedItem={{
                                    friend: selectedFriendItem,
                                    group: selectedGroupItem
                                }}
                                connection={connection.current}
                                isFriends={friendsTab}
                            />
                        </div>
                        <div className="tabs-panels-div">
                            <TabPanel
                                display={friendsTab ? "block" : "none"}
                                personGroupData={friendsData}
                                onItemClick={handleFriendItemClick}
                                isSelectedItem={selectedFriendItem}
                                connection={connection.current}
                                isFriends={true}
                            />
                            <TabPanel
                                display={friendsTab ? "none" : "block"}
                                personGroupData={groupsData}
                                onItemClick={handleGroupItemClick}
                                isSelectedItem={selectedGroupItem}
                                connection={connection.current}
                                isFriends={false}
                            />
                        </div>
                    </div>
                    <div className="messages-div">
                        <MessageHeaderBar
                            owner={owner.current}
                            personGroupData={friendsTab ? friendsData : groupsData}
                            selectedFriendItem={selectedFriendItem}
                            selectedGroupItem={selectedGroupItem}
                            isFriends={friendsTab}
                            logoutCallback={logout}
                        />
                        <MessagePanel
                            ownerId={owner.current}
                            friendGroupData={friendsTab ? friendsData : groupsData}
                            selectedFriendItem={selectedFriendItem}
                            selectedGroupItem={selectedGroupItem}
                            isFriends={friendsTab}
                        />
                        <MessageFooterBar
                            ownerId={owner.current}
                            friendGroupData={friendsTab ? friendsData : groupsData}
                            selectedFriendItem={selectedFriendItem}
                            selectedGroupItem={selectedGroupItem}
                            isFriends={friendsTab}
                            connection={connection.current}
                        />
                    </div>
                </>
                    :
                    <></>
            }
        </>
    );

}