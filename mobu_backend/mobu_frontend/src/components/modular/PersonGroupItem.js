import React,{ useEffect, useRef, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import GroupContextMenu from "../single_use/optionMenus/GroupContextMenu"
import FriendContextMenu from "../single_use/optionMenus/FriendContextMenu"

/**
 * 
 * Item de amigos ou grupos do utilizador
 * 
 * @param {*} PIProps propriedades do item : image, text
 * @param onClick gestor de clique no item
 * @param isGroup booleano de grupo 
 * @returns 
 */
export default function PersonGroupItem({owner, connections, isSelectedProp, PIProps, onClick, isGroup}){ 
    
    const[showMenu, setShowMenu] = useState(false);

    useEffect(() => {
        if(isSelectedProp){
            connections[0].invoke("AddToRoom", PIProps.info[isGroup ? 0 : 3] + "", connections[0].connectionId);
        }else{
            connections[0].invoke("RemoveFromRoom", PIProps.info[isGroup ? 0 : 3] + "", connections[0].connectionId);
        }
    },[PIProps.info, connections, isGroup, isSelectedProp])

    const handleMouseEnter = () => {
        setShowMenu(true);
    }

    const handleMouseOut = () => {
        setShowMenu(false);
    }

    return(
        <div
        onClick={e => {
            e.stopPropagation();
            onClick(PIProps.info);
        }}
        onMouseEnter={e => {
            e.stopPropagation();
            handleMouseEnter();
        }}
        onMouseLeave={e => {
            e.stopPropagation();
            handleMouseOut();
        }}
        className="person-group-item"
        style={isSelectedProp ? {background:"#c4dcf2"}: {background:"#8ab9e5"}}>
            <Avatar avatarProps={PIProps.image}/>
            <TopTextBottomText isSelected={isSelectedProp} TTBTProps={PIProps.text} fromParent="item"/>
            {isGroup ? <GroupContextMenu showMenuOnRightClick={showMenu}/> : <FriendContextMenu showMenuOnRightClick={showMenu}/>}
        </div>
    );

    
}