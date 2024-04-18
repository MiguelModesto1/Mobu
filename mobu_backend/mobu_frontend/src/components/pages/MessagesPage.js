import React,{ useState, useEffect, useRef } from "react";
import { useLocation } from 'react-router-dom';
import {HubConnection as signalR} from "@microsoft/signalr";
import TabsDiv from "../single_use/tabsDiv/TabsDiv";
import MessagesDiv from "../single_use/messagesDiv/MessagesDiv";

/**
 * 
 * Pagina de mensagens
 * 
 * @returns 
 */
export default function MessagesPage(){
    const queryParam = new URLSearchParams(window.location.href).get('email');
    const queryParamId = new URLSearchParams(window.location.href).get('id');

    const connections = useRef([
    new signalR.HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_HUB_URL + "/ChatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build()]);

    const owner = useRef();

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

        var queryParams=`?email=${queryParam}&id=${queryParamId}`;

        fetch(process.env.REACT_APP_API_URL + "/messages" + queryParams, options)
        .then(response => {
            if(response.status !== 404){
                return response.json();
            }else{
                window.location.assign("./error-404")
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
        start(connections.current);        
    });

    async function start(connection){
        try {
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

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
    
        <TabsDiv 
        owner={owner.current} 
        connections={connections.current} 
        selected={selectedItem} 
        childrenData={{friends: friendsData, groups: groupsData}} 
        displayFriends={friendsTab} 
        onItemClick={handleItemClick} 
        onHeaderClick={handleTabHeaderClick}/>
        
        <MessagesDiv 
        owner={owner.current} 
        text={{top: selectedItem.length !== 0 ? selectedItem[1] : "", bottom: selectedItem.length !== 0 ? friendsTab ? selectedItem[5] : selectedItem[3] : ""}} 
        messageContainersData={selectedItem} 
        connections={connections.current}/>

    </>);

    }