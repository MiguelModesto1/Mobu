import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import React, { useMemo, useEffect, useRef, useState } from "react";
import MessagesDiv from "../single_use/messagesDiv/MessagesDiv";
import TabsDiv from "../single_use/tabsDiv/TabsDiv";

/**
 * 
 * Pagina de mensagens
 * 
 * @returns 
 */
export default function MessagesPage() {

    const queryParamId = new URLSearchParams(window.location.search).get('id');

    const owner = useRef();

    const [connections, setConnections] = useState();
    const [selectedItem, setSelectedItem] = useState([]);
    const [friendsTab, setFriendsTab] = useState(true);
    const [friendsData, setFriendsData] = useState([]);
    const [groupsData, setGroupsData] = useState([]);

    useEffect(() =>{

        //buscar dados de mensagens do utilizador e povoar pagina

        var options = {
            method:"GET",
            redirect:"follow"
        }

        var queryParams=`?id=${queryParamId}`;

        fetch(process.env.REACT_APP_API_URL + "/messages" + queryParams, options)
        .then(response => {
            if(response.status !== 404){
                return response.json();
            }else{
                window.location.assign("/error-404")
            }
        })
        .then(data => {

            var friendsData = [];
            var groupsData = [];

            // recolher valores necessarios
            var friendsListLen = data.lengthFriendsList;

            for(let i=0; i<friendsListLen; i++){
                friendsData[i] = data.friends[i];
            }

            var groupsListLen = data.lengthGroupsList;

            for(let i=0; i<groupsListLen; i++){
                groupsData[i] = data.groups[i];
            }

            owner.current = data.ownerInfo;

            setFriendsData(friendsData);
            setGroupsData(groupsData);
        })
        .catch(err => console.error("error", err));

        // inicar conexao ao Hub de chat
        const conn =
            new HubConnectionBuilder()
                .withUrl(process.env.REACT_APP_HUB_URL + "/RealTimeHub", {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                    })
                .configureLogging(LogLevel.Debug)
                .build();
        conn.start();
        setConnections(conn);

        console.log(friendsData);
        console.log(groupsData);

    }, []);

    const start = async (connection) => {
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    };

    const handleTabHeaderClick = (friends) =>{
        if(friends){
            setFriendsTab(true);
        }else{
            setFriendsTab(false);
        }
    }

    function handleItemClick(itemProps){
        setSelectedItem(itemProps);
    }

    return(<>
    
        {/*<TabsDiv 
        owner={owner.current} 
        connections={connections} 
        selected={selectedItem} 
        childrenData={{friends: friendsData, groups: groupsData}} 
        displayFriends={friendsTab} 
        onItemClick={handleItemClick} 
        onHeaderClick={handleTabHeaderClick}/>
        
        <MessagesDiv 
        owner={owner.current} 
        text={{top: selectedItem.length !== 0 ? selectedItem[1] : "", bottom: selectedItem.length !== 0 ? friendsTab ? selectedItem[5] : selectedItem[3] : ""}} 
        messageContainersData={selectedItem} 
        connections={connections}/>*/}

    </>);

    }