import React, { useState } from "react";

/**
 * 
 * Componente para o avatar do utilizador.
 * 
 * @param {*} avatarProps propriedades do avatar: size, src, alt
 *
 */
export default function Avatar({ avatarProps }) {

    return (
        <div
            className="avatar-div"
            width={avatarProps.size}
            height={avatarProps.size}>
            <img
                className="img-fluid rounded-circle avatar"
                style={{
                    width: avatarProps.size,
                    height: avatarProps.size,
                    objectFit: "cover"
                }}
                src={avatarProps.src === "" || avatarProps.src === undefined ? undefined : avatarProps.src}
                alt={avatarProps.alt}
                height={avatarProps.size}
            />
        </div>
    );


}