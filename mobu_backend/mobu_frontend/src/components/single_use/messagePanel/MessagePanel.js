import React,{ useState,useEffect } from "react";
import MessageContainer from "../../modular/MessageContainer"
import "./MessagePanel.css"

/**
 * 
 * Painel de mensagens
 * 
 * @param {*} children filhos
 * @returns 
 */
export default function MessagePanel({owner, connections, childrenData}){ 

    const [messages, setMessages] = useState(childrenData[childrenData.length - 1]);

    useEffect(() => {
        const nextMessages = messages;
        connections[0].on("ReceiveMessage", function(user, message, idMsg){
            setMessages(nextMessages.push([message, parseInt(user), idMsg]))
    })}, [connections, messages]);


    /*const containers = messages.map((message) => {
        
        let cssClass;

        if(message[1] === owner){
            cssClass="owner"
        }else{
            cssClass="other-users"
        }

        return(
            <MessageContainer key={message[2]} children={message} fromParent={cssClass}/>
        );
    });*/

    return(
        <div className="message-panel">
            {/*containers*/}
        </div>
    );

    
}