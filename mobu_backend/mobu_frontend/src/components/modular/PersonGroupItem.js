import React,{ useState } from "react";
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
export default function PersonGroupItem({PIProps, onClick, isGroup}){ 
    
    const[showMenu, setShowMenu] = useState(false);
    const[isSelected, setIsSelected] = useState(false);

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
            setIsSelected(!isSelected);
            onClick(PIProps.key, isSelected);
        }}
        onMouseEnter={e => {
            e.stopPropagation();
            handleMouseEnter();
        }}
        onMouseLeave={e => {
            e.stopPropagation();
            handleMouseOut();
        }}
        className="person-item">
            <Avatar avatarProps={PIProps.image}/>
            <TopTextBottomText TTBTProps={PIProps.text}/>
            {isGroup ? <GroupContextMenu showMenuOnRightClick={showMenu}/> : <FriendContextMenu showMenuOnRightClick={showMenu}/>}
        </div>
    );

    
}