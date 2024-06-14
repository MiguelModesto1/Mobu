import React,{ useContext } from "react";
import PersonGroupItem from "./PersonGroupItem";
import FriendContextMenu from "../optionMenus/FriendContextMenu";

/**
 * 
 * Painel de itens de amigos/grupos
 * 
 * @param  
 * @returns 
 */
export default function TabPanel({ display, personGroupData, onItemClick, selectedItem, connection, isFriends, owner, expiry, onOverItem }) { 

    var personGroupItems = personGroupData.length !== 0 ?
        personGroupData.map(item => {
            return <PersonGroupItem
                key={item.ItemId}
                selectedItem={selectedItem}
                friendGroupData={item}
                onItemClick={onItemClick}
                connection={connection}
                isFriends={isFriends}
                onOverItem={onOverItem}
            />
        })
        :
        <></>;

    return (
        <div style={{ display: display, height: "42.438rem", backgroundColor: "lightblue" }} >
            <div className="tab-panel" style={{ overflow: "auto" }}>
                {personGroupItems}
            </div>
        </div>
        
    );

    
}