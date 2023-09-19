import React,{ useState } from "react";

/**
 * 
 * POR IMPLEMENTAR
 * 
 * @param  
 * @returns 
 */
export default function TabPanel({ownerId}){ 
    
    const [friendsGroupsList, setFriendsGroupsList] = useState([]);
    const [itemSelected, setItemSelected] = useState();

    const handleClick = (key, isSelected) => {
        return isSelected ? setItemSelected() : setItemSelected(key);
    }

    return(
        <div className="tab-panel">
            
        </div>
    );

    
}