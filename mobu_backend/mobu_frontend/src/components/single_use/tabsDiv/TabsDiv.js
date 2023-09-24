import React,{ useState } from "react";
import "./TabsDiv.css";
import FriendsTab from "../tabs/FriendsTab";
import GroupsTab from "../tabs/GroupsTab";

/**
 * 
 * 
 * @returns 
 */
export default function TabsDiv({owner, connections, selected, childrenData, displayFriends, onItemClick, onHeaderClick}){


    return(<>
    
        <FriendsTab 
        owner={owner} 
        connections={connections} 
        selected={selected} 
        text={"Amigos"} 
        childrenData={childrenData.friends} 
        onItemClick={onItemClick} 
        onHeaderClick={onHeaderClick} 
        panelDisplay={displayFriends ? "block" : "none"}/>
        
        <GroupsTab 
        owner={owner} 
        connections={connections} 
        selected={selected} 
        text={"Grupos"} 
        childrenData={childrenData.groups} 
        onItemClick={onItemClick} 
        onHeaderClick={onHeaderClick} 
        panelDisplay={displayFriends ? "none" : "block"}/>

    </>);

}