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
export default function GroupsTab({text, children}){
    
    return(
        <div className="tab">
           <TabHeader text={text} onClick={() => alert("Implementar onClick de GroupsTab")}/>
           <TabPanel >
                
           </TabPanel>
        </div>
    );

    
}