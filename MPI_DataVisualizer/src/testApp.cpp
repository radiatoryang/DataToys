
#include "testApp.h"

//--------------------------------------------------------------
void testApp::setup(){
    
    dBase.loadCities("cities.csv");
    dBase.loadSample(2000, "2000.csv");
    dBase.loadSample(2005, "2005.csv");
    dBase.loadSample(2010, "2010.csv");
    
    cout << dBase.getSample( dBase.getYearId(2000), dBase.getCityId("New York") ).pop << endl;
}

//--------------------------------------------------------------
void testApp::update(){
	
    
}

//--------------------------------------------------------------
void testApp::draw(){
	
}

//--------------------------------------------------------------
void testApp::keyPressed(int key){
	
}

//--------------------------------------------------------------
void testApp::keyReleased(int key){
}

//--------------------------------------------------------------
void testApp::mouseMoved(int x, int y ){
	
}

//--------------------------------------------------------------
void testApp::mouseDragged(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::mousePressed(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::mouseReleased(int x, int y, int button){
	
}

//--------------------------------------------------------------
void testApp::windowResized(int w, int h){
	
}

//--------------------------------------------------------------
void testApp::gotMessage(ofMessage msg){
	
}

//--------------------------------------------------------------
void testApp::dragEvent(ofDragInfo dragInfo){ 
	
}
