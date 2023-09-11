import React,{ useState } from "react";
import TabHeader from "../../modular/TabHeader";
import TabPanel from "../../modular/TabPanel";

export default function FriendsTab({text, children}){
    
    return(
        <div className="tab">
           <TabHeader text={text} onClick={() => alert("Implementar onClick de FriendsTab")}/>
           <TabPanel >
                
           </TabPanel>
        </div>
    );

    
}