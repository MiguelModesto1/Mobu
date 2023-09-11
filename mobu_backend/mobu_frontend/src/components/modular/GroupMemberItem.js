import React,{ useEffect, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

export default function GroupMemberItem({avatar, personId, personName, isAdmin, isEditing}){

    const [getExpel, setExpel] = useState(false);
    const [getAdminLabel, setAdminLabel] = useState(<></>);
    const [getButton, setButton] = useState(<></>);

    function handleExpelButtonClick(){
        if(getExpel){
            setExpel(true);
        }
    }

    useEffect(() => {
        if(isAdmin){

            setAdminLabel(
                <div
                marginRight={isEditing === false ? "80px" : "50px"} 
                className="group-member-admin-span-div">
                    <span className="group-member-admin-span">Admin</span>
                </div>
            );

            setButton(isEditing ?
                <Button
                text="Expulsar"
                color="#3b9ae1"
                onClick={handleExpelButtonClick} /> : <></>);
    
        }
    }, [])

    return(
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + personName,
                size:"40px"
            }}/>
            <TopTextBottomText 
            marginRight={isAdmin === false ? "375px" : "290px"}
            TTBTProps={{
                top:personId,
                bottom:personName
            }}/>
            {getAdminLabel}
            {getButton}
        </div>
    );

}