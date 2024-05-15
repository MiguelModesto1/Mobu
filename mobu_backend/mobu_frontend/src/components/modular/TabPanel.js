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
export default function TabPanel({ display, personGroupData, onItemClick, isSelectedItem, connection, isFriends }) { 

    var personGroupItems = personGroupData.length !== 0 ?
        personGroupData.map(item => {
            return <PersonGroupItem
                key={item.ItemId}
                isSelectedItem={isSelectedItem}
                friendGroupData={item}
                onItemClick={onItemClick}
                connection={connection}
                isFriends={isFriends}
            />
        })
        :
        <></>;

    return (
        <div className="tab-panel" style={{ display: display }}>
            {personGroupItems}
        </div>
    );

    
}