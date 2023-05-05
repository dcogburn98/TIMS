import * as THREE from 'three';
import { GLTFLoader } from './GLTFLoader.js';

// init
const gltfLoader = new GLTFLoader();

const camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 0.01, 10 );
camera.position.z = 1;

const scene = new THREE.Scene();
gltfLoader.load("GondolaUpright.glb", function (gltf) {

	scene.add(gltf.scene);

}, undefined, function (error) {

	console.error(error);

});

const geometry = new THREE.BoxGeometry( 0.2, 0.2, 0.2 );
const material = new THREE.MeshNormalMaterial();

const mesh = new THREE.Mesh( geometry, material );
//scene.add( mesh );

const renderer = new THREE.WebGLRenderer( { antialias: true } );
renderer.setSize( window.innerWidth, window.innerHeight );
renderer.setAnimationLoop( animation );
document.body.appendChild( renderer.domElement );

// animation

function animation( time ) {

	mesh.rotation.x = time / 800;
	mesh.rotation.y = time / 400;

	renderer.render( scene, camera );

}