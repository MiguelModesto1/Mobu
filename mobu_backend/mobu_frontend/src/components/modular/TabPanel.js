import React,{ useState } from "react";
import PersonGroupItem from "./PersonGroupItem";
import FriendContextMenu from "../single_use/optionMenus/FriendContextMenu";

/**
 * 
 * POR IMPLEMENTAR
 * 
 * @param  
 * @returns 
 */
export default function TabPanel({owner, connections, selected, friendGroupInfo, display, onItemClick}){ 
    
    const mapItems = friendGroupInfo.map((item) =>{
        
        return(
            <>
                <PersonGroupItem
                owner={owner}
                connections={connections}
                key={item[0]}
                isSelectedProp={item === selected}
                selected={selected}
                PIProps={{
                    info: item,
                    image: item[item.length - 2],
                    text:{
                        top: item[1],
                        bottom: item[item.length - 1][item[item.length - 1].length - 1]
                    }
                }}
                onClick={onItemClick}/>
                <FriendContextMenu owner={owner} id={item[0]} showMenuOnRightClick={"none"} />
            </>
            
        );
    })

    return(
        <div className="tab-panel" style={{display: display}}>
            {mapItems}
        </div>
    );

    
}