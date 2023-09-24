import React,{ useState } from "react";
import TabHeader from "../../modular/TabHeader";
import TabPanel from "../../modular/TabPanel";

/**
 * 
 * POR IMPLEMENTAR
 * 
 * @param 
 * @returns 
 */
export default function FriendsTab({owner, connections, selected, text, childrenData, onHeaderClick, onItemClick, panelDisplay}){
    
    return(
        <div className="tab">
           <TabHeader text={text} onClick={(e) =>{
                e.stopPropagation();
                onHeaderClick(true);
            }}/>
           <TabPanel owner={owner} connections={connections} selected={selected} onItemClick={onItemClick} friendGroupInfo={childrenData} display={panelDisplay}/>
        </div>
    );

    
}