import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import React, { useCallback, useLayoutEffect, useEffect, useRef, useState } from "react";
import TabHeader from "../modular/TabHeader";
import TabPanel from "../modular/TabPanel";
import MessageHeaderBar from "../single_use/messageHeaderBar/MessageHeaderBar";
import MessageFooterBar from "../single_use/messageFooterBar/MessageFooterBar";
import MessagePanel from "../single_use/messagePanel/MessagePanel";
import GroupContextMenu from "../optionMenus/GroupContextMenu";
import FriendContextMenu from "../optionMenus/FriendContextMenu";
import OwnerOptionMenu from "../optionMenus/OwnerOptionMenu";


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
    const expiry = useRef(Date.parse(sessionStorage.getItem("expiry")));
    const startDate = useRef(Date.parse(sessionStorage.getItem("startDate")));
    const timeout = useRef(0);

    const [showMenu, setShowMenu] = useState(false);
    const [tabsNumber, setTabsNumber] = useState(0);
    const [hasFetchedFriendsData, setHasFetchedFriendsData] = useState(false);
    const [hasFetchedGroupsData, setHasFetchedGroupsData] = useState(false);
    const [isFriendOverBlocked, setIsFriendOverBlocked] = useState(false);
    const [hasFriendOverBlockedMe, setHasFriendOverBlockedMe] = useState(false);
    const [overFriendItem, setOverFriendItem] = useState(0);
    const [overGroupItem, setOverGroupItem] = useState(0);
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
            ),
            BlockedThem: false,
            BlockedYou: false
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
            ),
            IsOwnerAdmin: false,
            HasLeft: false,
            WasExpelled: false
        }
    ]);

    /**
     * ouvir a saida de um grupo
     * @param {any} connection
     */
    const listenToGroupLeaving = useCallback((connection) => {
        connection.on("ReceiveLeaving", (group, message) => {
            //debugger;
            var leading = [];
            var trailing = [];
            var aux = {};

            for (var i = 0; i < groupsData.length; i++) {
                if (groupsData[i].IDSala + "" === group) {
                    aux = { ...groupsData[i] };
                    aux.HasLeft = true;
                    if (i === 0) {

                        trailing = groupsData.slice(1, groupsData.length);

                        setGroupsData([
                            { ...aux },
                            ...trailing
                        ]);
                    } else if (i === groupsData.length - 1) {

                        leading = groupsData.slice(0, groupsData.length - 1);

                        setGroupsData([
                            ...leading,
                            { ...aux }
                        ]);
                    } else {

                        leading = groupsData.slice(0, i);
                        trailing = groupsData.slice(i + 1, groupsData.length)

                        setGroupsData([
                            ...leading,
                            { ...aux },
                            ...trailing
                        ]);
                    }
                }
            }

            //setGroupsData([...aux]);

            console.log(message);
        });
    }, [groupsData]);

    /**
     * Ouvir resposta a pedidos
     * @param {any} connection
     */
    const listenToRequestReply = useCallback((connection) => {
        connection.on("ReceiveRequestReply", (replierObject, reply) => {

            if (reply) {

                var newFriend = {
                    ItemId: friendsData.length,
                    FriendId: replierObject.friendId,
                    FriendName: replierObject.friendName,
                    CommonRoomId: replierObject.commonRoomId,
                    ImageURL: replierObject.imageURL,
                    Messages: []
                }

                var aux = [...friendsData];
                aux.push([...newFriend]);

                setFriendsData([...aux]);
            }

        });
    },[friendsData]);

    /**
     * Ouvir rececao de expulsao
     * @param {any} connection
     */
    const listenToMemberExpelling = useCallback((connection) => {
        connection.on("ReceiveExpelling", (roomId, message) => {
            //debugger;
            var leading = [];
            var trailing = [];
            var aux = {};

            for (var i = 0; i < groupsData.length; i++) {
                if (groupsData[i].IDSala + "" === roomId) {
                    aux = { ...groupsData[i] };
                    aux.WasExpelled = true;
                    if (i === 0) {

                        trailing = groupsData.slice(1, groupsData.length);

                        setGroupsData([
                            { ...aux },
                            ...trailing
                        ]);
                    } else if (i === groupsData.length - 1) {

                        leading = groupsData.slice(0, groupsData.length - 1);

                        setGroupsData([
                            ...leading,
                            { ...aux }
                        ]);
                    } else {

                        leading = groupsData.slice(0, i);
                        trailing = groupsData.slice(i + 1, groupsData.length)

                        setGroupsData([
                            ...leading,
                            { ...aux },
                            ...trailing
                        ]);
                    }
                }
            }

            //setGroupsData([...aux]);

            console.log(message);
        })
    }, [groupsData]);

    useEffect(() => {

        //debugger;
        if (hasFetchedFriendsData || hasFetchedGroupsData) {
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

                    trailing = groupsData.slice(1, groupsData.length);

                    setGroupsData([
                        { ...aux },
                        ...trailing
                    ]);
                } else if (itemId === groupsData.length - 1) {

                    leading = groupsData.slice(0, groupsData.length - 1);

                    setGroupsData([
                        ...leading,
                        { ...aux }
                    ]);
                } else {

                    leading = groupsData.slice(0, itemId);
                    trailing = groupsData.slice(itemId + 1, groupsData.length)

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
            //debugger;
            fetch(process.env.REACT_APP_API_URL + "/messages" + queryParams, options)
                .then(response => {
                    if (response.status === 404) {
                        window.location.assign(`/search?id=${queryParamId}`);
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

                    // numero de separadores
                    var auxTabNum = 0;

                    // dados
                    //debugger;
                    if (data.friends.length !== 0) {
                        setFriendsData([...data.friends]);
                        setHasFetchedFriendsData(true);
                        auxTabNum++;
                        connection.current.invoke("AddConnection", [...data.friends][0].CommonRoomId + "");
                    }
                    else {
                        setFriendsTab(false);
                    }

                    if (data.groups.length !== 0) {
                        setGroupsData([...data.groups]);
                        setHasFetchedGroupsData(true);
                        auxTabNum++;
                        connection.current.invoke("AddConnection", [...data.groups][0].IDSala + "");
                    }

                    setTabsNumber(auxTabNum);

                    
                })
                .catch(err => console.error("error: ", err));

            connection.current =
                new HubConnectionBuilder().withUrl(process.env.REACT_APP_HUB_URL + "/RealTimeHub", {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                    .configureLogging(LogLevel.Debug)
                    .build();

            //tratar da conexao do signalR
            connection.current.start();
            logSignalRAccess(connection.current);
            listenToSignalRLeaving(connection.current);
            listenToSignalRMessages(connection.current);
            listenToSignalRGroupChange(connection.current);
            listenToGroupEntry(connection.current);
            listenToBlock(connection.current);
            listenToUnblock(connection.current);

            //verificar novo cookie
            document.addEventListener("mousemove", () => getNewCookie());
            document.addEventListener("keydown", () => getNewCookie());
            
            //debugger
            var expiryIntervalInit = expiry.current - startDate.current;
            //debugger
            if (expiryIntervalInit !== 15 * 1000 * 60) {
                window.location.assign("/");
            }

            var expiryInterval = expiry.current - Date.now();
            //debugger;
            timeout.current = setTimeout(() => {
                logout();
                window.location.assign("/");

            }, expiryInterval);
        }
    }, [lastMessageReceived]);

    useEffect(() => {
        //debugger;
        if (hasFetchedGroupsData) {
            listenToGroupLeaving(connection.current);
            listenToMemberExpelling(connection.current);
        }
        
    }, [hasFetchedGroupsData, listenToGroupLeaving, listenToMemberExpelling]);

    useEffect(() => {
        if (hasFetchedFriendsData)
            listenToRequestReply(connection.current);
    }, [hasFetchedFriendsData, listenToRequestReply]);

    useEffect(() => {

        if (friendsData.length === 0 && groupsData.length !== 0) {
            setFriendsTab(false);
        }

        if (groupsData.length === 0 && friendsData.length !== 0) {
            setFriendsTab(true);
        }

        if (friendsData.length === 0 && groupsData.length === 0) {
            window.location.assign(`/search?id=${queryParamId}`);
        }
    }, [friendsData.length, groupsData.length, queryParamId]);

    /**
     * Obter novo cookie de sessao depois deste chegar a meia-vida
     */
    const getNewCookie = async () => {

        console.log("getNewCookie!!");
        //debugger;
        var expiryInterval = expiry.current - Date.now();

        if (expiryInterval < (15 * 1000 * 60) / 2) {

            var options = {
                method: "GET",
                redirect: "follow",
                credentials: "include"
            }

            var queryParams = `?id=${queryParamId}`;
            await fetch(process.env.REACT_APP_API_URL + "/get-new-cookie" + queryParams, options)
                .then(response => {
                    if (response.status === 401) {
                        window.history.back();
                    }
                    return response.json();
                })
                .then(data => {
                    sessionStorage.setItem("expiry", data.expiryDate);
                    sessionStorage.setItem("startDate", data.startDate);
                    expiry.current = Date.parse(data.expiryDate);
                    startDate.current = Date.parse(data.sartDate);

                    expiryInterval = expiry.current - Date.now();

                    clearTimeout(timeout.current);
                    timeout.current = setTimeout(
                        () => {
                            logout();
                            window.location.assign("/");

                        }, expiryInterval);
                })
                .catch(err => console.error("error: ", err));
        }

    };

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
    const logout = async () => {

        await connection.current.stop();
        sessionStorage.removeItem("expiry");
        sessionStorage.removeItem("startDate");

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
     * mensagem do signalR ao desconectar
     * 
     * @param {any} connection
     */
    const listenToSignalRLeaving = (connection) => {
        connection.on("OnDisconnectedAsyncPrivate", message => {
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
            console.log("conex�o " + connectionId + " adicionada � sala " + roomId);
        });

        connection.on("RemovedFromGroup", (connectionId, roomId) => {
            console.log("conex�o " + connectionId + " removida da sala " + roomId);
        });
    };

    /**
     * Ouvir a entrada num grupo
     * @param {any} connection
     */
    const listenToGroupEntry = (connection) => {
        connection.on("ReceiveEntry", (group, message) => {
            console.log(message);
        });
    }

    /**
     * ouvir o bloqueio de um amigo
     * @param {any} connection
     */
    const listenToBlock = (connection) => {
        connection.on("ReceiveBlock", (fromUser) => {

            var aux = {};
            var trailing = [];
            var leading = [];

            for (var i = 0; i < friendsData.length; i++) {
                if (friendsData[i].FriendId + "" === fromUser) {
                    aux = { ...friendsData[i] }
                    aux.BlockedYou = true;
                    if (i === 0) {

                        trailing = friendsData.slice(1, friendsData.length);

                        setFriendsData([
                            { ...aux },
                            ...trailing
                        ]);
                    } else if (i === friendsData.length - 1) {

                        leading = friendsData.slice(0, friendsData.length - 1);

                        setFriendsData([
                            ...leading,
                            { ...aux }
                        ]);
                    } else {

                        leading = friendsData.slice(0, i);
                        trailing = friendsData.slice(i + 1, friendsData.length)

                        setFriendsData([
                            ...leading,
                            { ...aux },
                            ...trailing
                        ]);
                    }
                    break;
                }
            }
        });
    };

    /**
     * ouvir o desbloqueio de um amigo
     * @param {any} connection
     */
    const listenToUnblock = (connection) => {
        connection.on("ReceiveUnblock", (fromUser) => {

            var aux = {};
            var trailing = [];
            var leading = [];

            for (var i = 0; i < friendsData.length; i++) {
                if (friendsData[i].FriendId + "" === fromUser) {
                    aux = { ...friendsData[i] }
                    aux.BlockedYou = false;
                    if (i === 0) {

                        trailing = friendsData.slice(1, friendsData.length);

                        setFriendsData([
                            { ...aux },
                            ...trailing
                        ]);
                    } else if (i === friendsData.length - 1) {

                        leading = friendsData.slice(0, friendsData.length - 1);

                        setFriendsData([
                            ...leading,
                            { ...aux }
                        ]);
                    } else {

                        leading = friendsData.slice(0, i);
                        trailing = friendsData.slice(i + 1, friendsData.length)

                        setFriendsData([
                            ...leading,
                            { ...aux },
                            ...trailing
                        ]);
                    }
                    break;
                }
            }
        });
    };

    

    

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

    /**
     * mudanca de cursor do rato (amigos)
     * @param {any} itemKey
     */
    function handleOverFriendItem(itemKey) {
        //debugger;
        setOverFriendItem(itemKey);
        setIsFriendOverBlocked(friendsData[itemKey].BlockedThem);
        setHasFriendOverBlockedMe(friendsData[itemKey].BlockedYou);
    }

    /**
     * bloquear/desbloquear amigo
     * @param {any} itemId
     * @param {any} boolValue
     */
    function handleBlock(itemId, boolValue) {
        var aux = {};
        var trailing = [];
        var leading = [];

        aux = { ...friendsData[itemId] }
        aux.BlockedThem = boolValue;
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
    }

    /**
     * mudanca de cursor do rato (grupos)
     * @param {any} itemKey
     */
    function handleOverGroupItem(itemKey) {
        //debugger
        setOverGroupItem(itemKey);
    }

    /**
     * clique no icone de menu
     */
    const handleMenuIconClick = () => {
        setShowMenu(!showMenu);
    }

    return (
        <div style={{ maxWidth:"99.2%" }}>
            {
                hasFetchedFriendsData || hasFetchedGroupsData ?
                    <div className="row">
                        <div className="col-lg-2 px-0">
                            <div className="navbar py-0 d-flex justify-content-evenly">
                                {hasFetchedFriendsData && friendsData.length !== 0 &&
                                    <TabHeader
                                        tabsNumber={tabsNumber}
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
                                }
                                {hasFetchedGroupsData && groupsData.length !== 0 &&
                                    <TabHeader
                                        tabsNumber={tabsNumber}
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
                                }
                                
                            </div>
                            <div className="tabs-div">
                                
                                {hasFetchedFriendsData && friendsData.length !== 0 &&
                                    <TabPanel
                                        display={friendsTab ? "block" : "none"}
                                        personGroupData={friendsData}
                                        onItemClick={handleFriendItemClick}
                                        selectedItem={selectedFriendItem}
                                        connection={connection.current}
                                        isFriends={true}
                                        onOverItem={handleOverFriendItem}
                                    />
                                }
                                {hasFetchedGroupsData && groupsData.length !== 0 &&
                                    <TabPanel
                                        display={friendsTab ? "none" : "block"}
                                        personGroupData={groupsData}
                                        onItemClick={handleGroupItemClick}
                                        selectedItem={selectedGroupItem}
                                        connection={connection.current}
                                        isFriends={false}
                                        onOverItem={handleOverGroupItem}
                                    />
                                }
                            </div>
                        </div>
                        <div className="col-lg-10 px-0">
                            <MessageHeaderBar
                                personGroupData={friendsTab ? friendsData : groupsData}
                                selectedFriendItem={selectedFriendItem}
                                selectedGroupItem={selectedGroupItem}
                                isFriends={friendsTab}
                                onMenuIconClick={handleMenuIconClick}
                            />
                            <OwnerOptionMenu
                                owner={owner.current}
                                showMenu={showMenu ? "block" : "none"}
                                connection={connection.current}
                                logoutCallback={() => logout()}
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
                        {friendsTab ?
                            friendsData.length !== 0 &&
                            <FriendContextMenu
                                itemId={overFriendItem}
                                isFriendOverBlocked={isFriendOverBlocked}
                                hasFriendOverBlockedMe={hasFriendOverBlockedMe}
                                owner={owner.current}
                                onBlock={handleBlock}
                                id={friendsData[overFriendItem].FriendId}
                                connection={connection.current}
                            />
                            :
                            groupsData.length !== 0 &&
                            groupsData[overGroupItem] !== null &&
                            groupsData[overGroupItem] !== undefined &&
                            <GroupContextMenu
                                hasLeft={groupsData[overGroupItem].HasLeft}
                                wasExpelled={groupsData[overGroupItem].WasExpelled}
                                owner={owner.current}
                                isOwnerAdmin={groupsData[overGroupItem].IsOwnerAdmin}
                                id={groupsData[overGroupItem].IDSala}
                                connection={connection.current}
                            />
                        }
                    </div>
                    :
                    <></>
            }
        </div>
    );

}