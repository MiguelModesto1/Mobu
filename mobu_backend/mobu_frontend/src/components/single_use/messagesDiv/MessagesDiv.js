import React,{ useEffect, useState } from "react";

import "./MessagesDiv.css";
import MessageFooterBar from "../messageFooterBar/MessageFooterBar";
import MessageHeaderBar from "../messageHeaderBar/MessageHeaderBar";
import MessagePanel from "../messagePanel/MessagePanel";


/**
 * 
 * 
 * @returns 
 */
export default function MessagesDiv({owner, text, messageContainersData, connections}){

    return(<>

        <MessageHeaderBar owner={owner} text={text} connections={connections}/>
        <MessagePanel owner={owner} childrenData={messageContainersData} connections={connections}/>
        <MessageFooterBar owner={owner} footerProps={messageContainersData} connections={connections}/>

    </>);

}